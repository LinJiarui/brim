import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom
import xlrd
from dict_sap_to_owl_v2 import dict_sap_to_owl

#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-##-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# string constant
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-##-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
ns_brim = "http://eil.stanford.edu/ontologies/brim"
ns_base = "http://eil.stanford.edu/ontologies/brim"
ns_rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns"
ns_owl = "http://www.w3.org/2002/07/owl"
ns_xml = "http://www.w3.org/XML/1998/namespace"
ns_xsd = "http://www.w3.org/2001/XMLSchema"
ns_rdfs = "http://www.w3.org/2000/01/rdf-schema"

ns_base_s = ns_base+"#"
ns_brim_s = ns_brim+"#"
ns_rdf_s = ns_rdf+'#'
ns_owl_s = ns_owl+'#'
ns_xml_s = ns_xml+'#'
ns_xsd_s = ns_xsd+'#'
ns_rdfs_s = ns_rdfs+'#'

ns_base_p = "{"+ns_base_s+"}"
ns_brim_p = "{"+ns_brim_s+"}"
ns_rdf_p = "{"+ns_rdf_s+"}"
ns_owl_p = "{"+ns_owl_s+"}"
ns_xml_p = "{"+ns_xml_s+"}"
ns_xsd_p = "{"+ns_xsd_s+"}"
ns_rdfs_p = "{"+ns_rdfs_s+"}"

inputFileName = 'New_TRB4_cleaned_modal_only.xlsx'


#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# global variable
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
dataEntity = {}
indexRecord = {}
duplicationAllowList = ['hasBridgeLayoutHorizontalSegment', 
                        'hasBridgeLayoutVerticalSegment',
                        'hasFrequencyFunctionValue',
                        'hasPeriodFunctionValue',
                        'hasTimeSeriesFunctionValue',
                        'hasLaneSegment',
                        'hasAreaJointOffset',
                        'hasSubSectionPlate']

varForAllowingDuplication = [0]

sapClassIDs = {'Angle':'LineSectionAngle',
               'Tee':'LineSectionTShape',
               'Channel':'LineSectionCShape',
               'SD Section':'LineUserDefinedSection'}


#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# functions
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

def Prettify(elem):
    rough_string = ET.tostring(elem)
    reparsed = minidom.parseString(rough_string)
    return reparsed.toprettyxml(indent="\t")


def DefineNameSpaces():
	ns_dict = {'':ns_base_s,
				'rdf':ns_rdf_s,
				'owl':ns_owl_s,
				'xml':ns_xml,
				'xsd':ns_xsd_s,
				'rdfs':ns_rdfs_s,
				'brim':ns_brim_s}
	for key in ns_dict:
		ET.register_namespace(key, ns_dict[key])


def ImportOntology(root, uri):
	element = ET.Element("{0}Ontology".format(ns_owl_p))
	element.set("{0}about".format(ns_rdf_p),ns_base_s)
	element2 = ET.Element("{0}imports".format(ns_owl_p))
	element2.set("{0}resource".format(ns_rdf_p),uri)
	element.append(element2)
	root.append(element)



#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# XLSX Parser
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

def ScanAttributes(dictionary, sheet):
	ncol = sheet.ncols
	columnList = []
	for i in range(0, ncol):
		columnList.append(sheet.cell(1,i).value)
	columnDict = {}
	for key in dictionary:
		if key in columnList:
			columnDict[columnList.index(key)] = dictionary[key]
	return columnDict


def ParseDataProperty(sheet, rowIdx, columnIdxDict):
	result = {}

	for key in columnIdxDict:
		value = sheet.cell(rowIdx, key).value
		valueType = columnIdxDict[key][1]
		if valueType == int:
			if value == '':
				continue
				# value = 0
			else:
				value = int(value)
			dataType = "{0}integer".format(ns_xsd_s)
		elif valueType == str:
			try:
				value = str(int(value))
			except:
				value = value
			dataType = "{0}string".format(ns_xsd_s)
		elif valueType == bool:
			value = 'true' if value in ['Yes'] else 'false'
			dataType = "{0}boolean".format(ns_xsd_s)
		elif valueType == float:
			if value == '':
				continue
				# value = 0
			else:
				value = float(value)
			dataType = "{0}double".format(ns_xsd_s)
		result[columnIdxDict[key][0]] = [value, dataType]

	return result


def ParseObjectProperty(sheet, rowIdx, columnIdxDict):
	result = {}
	for key in columnIdxDict:
		value = sheet.cell(rowIdx, key).value
		valueType = columnIdxDict[key][1]
		if type(value) == int or type(value) == float:
			value = str(int(value))
		result[columnIdxDict[key][0]] = [value, valueType]
	return result


def GetIdentifier(classID, attributes):
	isDuplicated = False
	if classID in dataEntity:
		for existingID in dataEntity[classID]:
			if dataEntity[classID][existingID] == attributes:
				isDuplicated = True
				return existingID, isDuplicated
		# indexRecord[classID] += 1
		if 'ID' in attributes:
			identifier = "{0}__{1}".format(classID,attributes['ID'][0])
		else:
			indexRecord[classID] += 1
			identifier = "{0}__{1}".format(classID,str(int(indexRecord[classID])))
		return identifier, isDuplicated
	else:
		if 'ID' in attributes:
			identifier = "{0}__{1}".format(classID,attributes['ID'][0])
		else:
			indexRecord[classID] = 0
			identifier = "{0}__{1}".format(classID,'0')
		dataEntity[classID] = {}
		return identifier, isDuplicated
		

def AssignForeignKeyEntity(classID, identifier, foreignKeys):
	for objectProperty in foreignKeys:
		if objectProperty in duplicationAllowList:
			objectPropertyKey = objectProperty + '____' + str(int(varForAllowingDuplication[0]))
			varForAllowingDuplication[0] += 1
		else:
			objectPropertyKey = objectProperty
		targetClassIDCandidate = foreignKeys[objectProperty][1]
		targetClassID = DecideClassID(targetClassIDCandidate, {'ID': foreignKeys[objectProperty]})
		targetIdentifier = "{0}__{1}".format(targetClassID,foreignKeys[objectProperty][0])
		if targetClassID in dataEntity:
			if targetIdentifier not in dataEntity[targetClassID]:
				dataEntity[targetClassID][targetIdentifier] = {}
		else:
			dataEntity[targetClassID] = {}
			dataEntity[targetClassID][targetIdentifier] = {}
		if classID in dataEntity:
			if identifier in dataEntity[classID]:
				dataEntity[classID][identifier][objectPropertyKey] = [targetIdentifier, None]
			else:
				dataEntity[classID][identifier] = {}
				dataEntity[classID][identifier][objectPropertyKey] = [targetIdentifier, None]
		else:
			dataEntity[classID] = {}
			dataEntity[classID][identifier] = {}
			dataEntity[classID][identifier][objectPropertyKey] = [targetIdentifier, None]


def AssignInvForeignKeyEntity(invForeignKeys, targetID):
	for objectProperty in invForeignKeys:
		if objectProperty in duplicationAllowList:
			objectPropertyKey = objectProperty + '____' + str(int(varForAllowingDuplication[0]))
			varForAllowingDuplication[0] += 1
		else:
			objectPropertyKey = objectProperty

		classIDCandidate = invForeignKeys[objectProperty][1]
		classID = DecideClassID(classIDCandidate, {'ID': invForeignKeys[objectProperty]})
		identifier = "{0}__{1}".format(classID,invForeignKeys[objectProperty][0])

		if classID in dataEntity:
			if identifier in dataEntity[classID]:
				dataEntity[classID][identifier][objectPropertyKey] = [targetID, None]
			else:
				dataEntity[classID][identifier] = {}
				dataEntity[classID][identifier][objectPropertyKey] = [targetID, None]
		else:
			dataEntity[classID] = {}
			dataEntity[classID][identifier] = {}
			dataEntity[classID][identifier][objectPropertyKey] = [targetID, None]


def DecideClassID(classIDCandidate, attributes):
	if len(classIDCandidate) == 1:
		return classIDCandidate[0]
	elif 'classID' in attributes:
		if attributes['classID'][0]	in sapClassIDs:
			return sapClassIDs[attributes['classID'][0]]
		else:
			return attributes['classID'][0]
	else:
		for classID in classIDCandidate:
			classSet = [attributes[idx][0] for idx in attributes]
			identifier = "{0}__{1}".format(classID, attributes['ID'][0])
			if identifier in dataEntity[classID]:
				return classID


def ParseSheet(sheet, classIDCandidate, attributeIndex, foriegnKeyIndex, invForiegnKeyIndex):
	# nrow = sheet.nrows
	for i in range(3,min(sheet.nrows,9999)):
		# Read the i-th row
		attributes = ParseDataProperty(sheet, i, attributeIndex)
		foreignKeys = ParseObjectProperty(sheet, i, foriegnKeyIndex)
		invForeignKeys = ParseObjectProperty(sheet, i, invForiegnKeyIndex)
		# Get class ID
		classID = DecideClassID(classIDCandidate, attributes)

		# Check duplication and create/get ID
		identifier, isDuplicated = GetIdentifier(classID, attributes)		

		# Apply data attributes (i.e., data property)
		if not isDuplicated:
			if identifier not in dataEntity[classID]:
				dataEntity[classID][identifier] = attributes
			else:
				dataEntity[classID][identifier].update(attributes)

		# Apply foreign key (i.e., object property)
		AssignForeignKeyEntity(classID, identifier, foreignKeys)

		# Apply inverse foreign key (i.e., inverse object property)
		AssignInvForeignKeyEntity(invForeignKeys, identifier)


def ParseMultiRowAttributeSheet(sheet, sheetName, classIDCandidate, attributeIndex, foriegnKeyIndex, invForiegnKeyIndex):
	localIdx = 1
	prevID = ''
	mergedAttributes = {}
	for i in range(3,min(sheet.nrows,9999)):
		thisID = sheet.cell(i,invForiegnKeyIndex.keys()[0]).value
		if prevID != thisID: 
			if prevID == '':
				prevID = thisID
			else:
				# Get class ID
				classID = DecideClassID(classIDCandidate, mergedAttributes)
				# Check duplication and create/get ID
				identifier, isDuplicated = GetIdentifier(classID, mergedAttributes)
				# Apply data attributes (i.e., data property)
				if not isDuplicated:
					if identifier not in dataEntity[classID]:
						dataEntity[classID][identifier] = mergedAttributes
					else:
						dataEntity[classID][identifier].update(mergedAttributes)
				# Apply foreign key (i.e., object property)
				AssignForeignKeyEntity(classID, identifier, foreignKeys)
				# Apply inverse foreign key (i.e., inverse object property)
				AssignInvForeignKeyEntity(invForeignKeys, identifier)
				localIdx = 1
				mergedAttributes = {}
				prevID = thisID

		colIdx = attributeIndex.keys()
		colIdx.sort()
		for col in colIdx:
			if sheet.cell(i,col).value == '':
				break
			
			attributes = ParseDataProperty(sheet, i, attributeIndex)
			foreignKeys = ParseObjectProperty(sheet, i, foriegnKeyIndex)
			invForeignKeys = ParseObjectProperty(sheet, i, invForiegnKeyIndex)

			for key in attributes:
				mergedAttributes[key+str(int(localIdx))] = attributes[key]
				localIdx += 1

	# Get class ID
	classID = DecideClassID(classIDCandidate, mergedAttributes)
	# Check duplication and create/get ID
	identifier, isDuplicated = GetIdentifier(classID, mergedAttributes)
	# Apply data attributes (i.e., data property)
	if not isDuplicated:
		if identifier not in dataEntity[classID]:
			dataEntity[classID][identifier] = mergedAttributes
		else:
			dataEntity[classID][identifier].update(mergedAttributes)
	# Apply foreign key (i.e., object property)
	AssignForeignKeyEntity(classID, identifier, foreignKeys)
	# Apply inverse foreign key (i.e., inverse object property)
	AssignInvForeignKeyEntity(invForeignKeys, identifier)


def parseMultiRowForeignKeySheet(sheet, sheetName, classIDCandidate, attributeIndex, foriegnKeyIndex, invForiegnKeyIndex):
	localIdx = 1
	prevID = ''
	mergedForeignKeys = {}
	for i in range(3,min(sheet.nrows,9999)):
		thisID = sheet.cell(i,attributeIndex.keys()[0]).value
		if prevID != thisID: 
			if prevID == '':
				prevID = thisID
			else:
				# Get class ID
				classID = DecideClassID(classIDCandidate, attributes)
				# Check duplication and create/get ID
				identifier, isDuplicated = GetIdentifier(classID, attributes)
				# Apply data attributes (i.e., data property)
				if not isDuplicated:
					if identifier not in dataEntity[classID]:
						dataEntity[classID][identifier] = attributes
					else:
						dataEntity[classID][identifier].update(attributes)
				# Apply foreign key (i.e., object property)
				AssignForeignKeyEntity(classID, identifier, mergedForeignKeys)
				# Apply inverse foreign key (i.e., inverse object property)
				AssignInvForeignKeyEntity(invForeignKeys, identifier)
				localIdx = 1
				mergedForeignKeys = {}
				prevID = thisID

		colIdx = foriegnKeyIndex.keys()
		colIdx.sort()
		for col in colIdx:
			if sheet.cell(i,col).value == '':
				break
			
			attributes = ParseDataProperty(sheet, i, attributeIndex)
			partialForeignKeyIndex = {}
			partialForeignKeyIndex[col] = foriegnKeyIndex[col]
			foreignKeys = ParseObjectProperty(sheet, i, partialForeignKeyIndex)
			invForeignKeys = ParseObjectProperty(sheet, i, invForiegnKeyIndex)
			for key in foreignKeys:
				mergedForeignKeys[key+str(int(localIdx))] = foreignKeys[key]
				localIdx += 1

	# Get class ID
	classID = DecideClassID(classIDCandidate, attributes)
	# Check duplication and create/get ID
	identifier, isDuplicated = GetIdentifier(classID, attributes)
	# Apply data attributes (i.e., data property)
	if not isDuplicated:
		if identifier not in dataEntity[classID]:
			dataEntity[classID][identifier] = attributes
		else:
			dataEntity[classID][identifier].update(attributes)
	# Apply foreign key (i.e., object property)
	AssignForeignKeyEntity(classID, identifier, mergedForeignKeys)
	# Apply inverse foreign key (i.e., inverse object property)
	AssignInvForeignKeyEntity(invForeignKeys, identifier)


#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# Owl editor
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

def DefineClassType(element, classID):
	objectClass = ET.Element("{0}type".format(ns_rdf_p))
	objectClass.set("{0}resource".format(ns_rdf_p),"{0}{1}".format(ns_brim_s,classID))
	element.append(objectClass)

def DefineDataProperty(element, propertyID, value, dataType):
	newProperty = ET.Element("{0}{1}".format(ns_brim_p,propertyID))
	newProperty.set("{0}datatype".format(ns_rdf_p),dataType)
	newProperty.text = str(value)
	element.append(newProperty)

def DefineObjectProperty(element, propertyID, value):
	# print propertyID
	if '____' in propertyID:
		propertyID = propertyID.split('____')[0]

	# print propertyID
	newProperty = ET.Element("{0}{1}".format(ns_brim_p,propertyID))
	newProperty.set("{0}resource".format(ns_rdf_p),"{0}{1}".format(ns_brim_s,value))
	# print newProperty
	element.append(newProperty)	

def CreateIndividuals(root):
	for classID in dataEntity:
		classIndividuals = dataEntity[classID]
		for individualID in classIndividuals:
			element = ET.Element("{0}NamedIndividual".format(ns_owl_p))
			element.set("{0}about".format(ns_rdf_p),"{0}{1}".format(ns_brim_s,individualID))
			properties = classIndividuals[individualID]
			# Class type
			DefineClassType(element, classID)
			for propertyID in properties:
				# Ignore ID
				if propertyID == 'ID' or propertyID == 'classID' :
					continue
				# Data property
				elif properties[propertyID][1] != None:
					DefineDataProperty(element, propertyID, properties[propertyID][0], properties[propertyID][1])
				# Object property
				else:
					DefineObjectProperty(element, propertyID, properties[propertyID][0])				
			root.append(element)



#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#
# main
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#

if __name__ == '__main__':

	# Open data file
	book = xlrd.open_workbook(inputFileName)

	# Prepare xml (owl) tree
	DefineNameSpaces()
	root = ET.Element("{0}RDF".format(ns_rdf_p))
	ImportOntology(root, ns_brim)
	
	for sheetInfo in dict_sap_to_owl:

		# Get column index of data entities
		try:
			sheet = book.sheet_by_name(sheetInfo['sheetName'])
		except:
			print "Spreadsheet {0} does not exist.".format(sheetInfo['sheetName'])
		else:
			classIDCandidate = sheetInfo['class']
			attributeIndex = ScanAttributes(sheetInfo['attribute'], sheet)
			foriegnKeyIndex = ScanAttributes(sheetInfo['foreignKey'], sheet)
			invForiegnKeyIndex = ScanAttributes(sheetInfo['invForeignKey'], sheet)

			if sheetInfo['sheetName'] == 'Area Overwrites - Joint Offsets':
				ParseMultiRowAttributeSheet(sheet, sheetInfo['sheetName'], classIDCandidate, attributeIndex, foriegnKeyIndex, invForiegnKeyIndex)
			elif sheetInfo['sheetName'] == 'Connectivity - Area':
				parseMultiRowForeignKeySheet(sheet, sheetInfo['sheetName'], classIDCandidate, attributeIndex, foriegnKeyIndex, invForiegnKeyIndex)
			else:
			# Parse data entities in a sheet 
				ParseSheet(sheet, classIDCandidate, attributeIndex, foriegnKeyIndex, invForiegnKeyIndex)

	# print dataEntity
	# Construction owl individuals
	CreateIndividuals(root)

	# print dataEntity
	# print indexRecord
	# Print result
	result = Prettify(root)
	# print result

	f = open("testoutput.owl", 'w')
	f.write(result)
	f.close()

