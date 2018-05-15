
# prefix = 'http://eil.stanford.edu/ontologies/brim#'

# SKipped: 


##### *** Program Control ***

# MatProp 03e - Rebar Data
# MatProp 03f - Tendon Data
# Case - Direct Hist 1 - General
# Case - Direct Hist 2 - Loads
# Case - Direct Hist 3 - Damping
# Case - Direct Hist 4 - Int Pars
# Case - Modal 1 - General
# Case - Static 1 - Load Assigns
# Constraint Definitions - Body
# Function - PSD - User
# Function - Resp Spect - User
# Function - Steady State - User
# Function - Time History - User
# Grid Lines
# Joint Loads - Force
# *** SD 30 - Fiber General


dict_sap_to_owl = [
	{
		'sheetName': 'Area Auto Mesh Assignments',
		'class': ['AreaAutoMesh'],
		'attribute': {
			'LocalEdge': ['hasLocalEdge', bool],
			'PtsFromLine': ['hasPointFromLine', bool],
			'SuppFace': ['hasSupportFace', bool],
			'PtsFromPt': ['hasPointFromPoint', bool],
			'SuppEdge': ['hasSupportEdge', bool],
			'SubMesh': ['hasSubMesh', bool],
			'LocalFace': ['hasLocalFace', bool],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Area': ['hasAreaAutoMesh', ['PlanarElement']],
		},
	},

	{
		'sheetName': 'Area Edge Constraint Assigns',
		'class': ['AreaEdgeConstraint'],
		'attribute': {
			'Constrained': ['hasConstrainedEdge', bool],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Area': ['hasAreaEdgeConstraint', ['PlanarElement']],
		},
	},

	{
		'sheetName': 'Area Overwrites - Joint Offsets',
		'class': ['AreaJointOffset'],
		'attribute': {
			'Offset1': ['hasAreaJointOffsetValue', float],
			'Offset2': ['hasAreaJointOffsetValue', float],
			'Offset3': ['hasAreaJointOffsetValue', float],
			'Offset4': ['hasAreaJointOffsetValue', float],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Area': ['hasAreaJointOffset', ['PlanarElement']],
		},
	},


	# {
	# 	'sheetName': 'Area Spring Assignments',
	# 	'class': ['AreaSpring'],
	# 	'attribute': {
	# 		'Type': ['hastype', str],
	# 		'Stiffness': ['hasStiffness', float],
	# 		'SimpleType': ['hasSubType', str],
	# 		'Face': ['hasFace', str],
	# 		'Dir1Type': ['hasDirection1Type', str],
	# 		'NormalDir': ['hasNormalDirection', str],
	# 	},
	# 	'foreignKey': {
	# 	},
	# 	'invForeignKey': {
	# 		'Area': ['hasAreaSpring', ['PlanarElement']],
	# 	},
	# },

	{
		'sheetName': 'MatProp 01 - General',
		'class': ['Concrete', 'Steel', 'Rebar', 'Tendon'],
		'attribute': {
			'TempDepend': ['hasTemperatureDependency', bool],
			'Material': ['ID', str],
			'Type': ['classID', str],
		},
		'foreignKey': {
		},
		'invForeignKey': {
		},
	},

	{
		'sheetName': 'MatProp 02 - Basic Mech Props',
		'class': ['Concrete', 'Steel', 'Rebar', 'Tendon'],
		'attribute': {
			'Material': ['ID', str],
			'A1': ['hasThermalExpansionCoefficient', float],
			'UnitWeight': ['hasUnitWeight', float],
			'UnitMass': ['hasUnitMass', float],
			'U12': ['hasPoissonRatio', float],
			'E1': ['hasElasticModulus', float],
			'G12': ['hasShearModulus', float],
		},
		'foreignKey': {
		},
		'invForeignKey': {
		},
	},

	{
		'sheetName': 'MatProp 03a - Steel Data',
		'class': ['Steel'],
		'attribute': {
			'SSCurveOpt': ['hasStrainStressCurveOption', str],
			'SMax': ['hasStrainAtMaximumStress', float],
			'Fu': ['hasMinimumTensileStress', float],
			'FinalSlope': ['hasFinalSlope', float],
			'SRup': ['hasStrainAtRupture', float],
			'Fy': ['hasMinimumYieldStress', float],
			'Material': ['ID', str],
			'SHard': ['hasStrainAtOnsetOfStrainHardening', float],
			'EffFy': ['hasExpectedYieldStress', float],
			'EffFu': ['hasExpectedTensileStress', float],
			'SSHysType': ['hasHysteresisType', str],
		},
		'foreignKey': {
		},
		'invForeignKey': {
		},
	},

	{
		'sheetName': 'MatProp 03b - Concrete Data',
		'class': ['Concrete'],
		'attribute': {
			'SSCurveOpt': ['hasStrainStressCurveOption', str],
			'LtWtConc': ['hasLightWeightConcrete', bool],
			'Material': ['ID', str],
			'FinalSlope': ['hasFinalCompressionSlope', float],
			'DAngle': ['hasDilatationalAngle', float],
			'SFc': ['hasStrainAtUnconfinedCompressiveStrength', float],
			'Fc': ['hasExpectedConcreteCompressiveStrength', float],
			'SCap': ['hasUltimateUnconfinedStrainCapacity', float],
			'SSHysType': ['hasHysteresisType', str],
			'FAngle': ['hasFrictionAngle', float],
		},
		'foreignKey': {
		},
		'invForeignKey': {
		},
	},

	# {
	# 	'sheetName': 'MatProp 06 - Damping Parameters',
	# 	'class': ['Concrete', 'Steel', 'Rebar', 'Tendon'],
	# 	'attribute': {
	# 		'VisStiff': ['hasViscousProportionalDampingStiffenessCoefficient', float],
	# 		'HysMass': ['hasHystereticProportionalDampingMassCoefficient', float],
	# 		'Material': ['ID', str],
	# 		'HysStiff': ['hasHystereticProportionalDampingStiffnessCoefficient', float],
	# 		'VisMass': ['hasViscousProportionalDampingMassCoefficient', float],
	# 		'ModalRatio': ['hasModalRatio', float],
	# 	},
	# 	'foreignKey': {
	# 	},
	# 	'invForeignKey': {
	# 	},
	# },

	{
		'sheetName': 'Area Section Properties',
		'class': ['Shell'],
		'attribute': {
			'V13Mod': ['hasModificationFactorV13', float],
			'F11Mod': ['hasModificationFactorF11', float],
			'AreaType': ['classID', str],
			'WMod': ['hasModificationFactorW', float],
			'DrillDOF': ['hasDrillDOF', bool],
			'Section': ['ID', str],
			'V23Mod': ['hasModificationFactorV13', float],
			'Arc': ['hasArc', float],
			'M22Mod': ['hasModificationFactorM22', float],
			'MatAngle': ['hasMaterialAngle', float],
			'MMod': ['hasModificationFactorM', float],
			'F22Mod': ['hasModificationFactorF22', float],
			'M11Mod': ['hasModificationFactorM11', float],
			'M12Mod': ['hasModificationFactorM12', float],
			'F12Mod': ['hasModificationFactorF12', float],
			'Thickness': ['hasThickness', float],
			'BendThick': ['hasBendThickness', float],
		},
		'foreignKey': {
			'Material': ['hasMaterial', ['Concrete', 'Steel', 'Rebar', 'Tendon']],
		},
		'invForeignKey': {
		},
	},

	{
		'sheetName': 'Area Section Assignments',
		'class': ['Shell', 'Plate'],
		'attribute': {
			'Section': ['ID', str],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Area': ['hasAreaSection', ['PlanarElement']],
		},
	},

	{
		'sheetName': 'Coordinate Systems',
		'class': ['Point'],
		'attribute': {
			'Y': ['hasY', float],
			'X': ['hasX', float],
			'Z': ['hasZ', float],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Name': ['hasOrigin', ['CoordinateSystem']],
		},
	},

	{
		'sheetName': 'Coordinate Systems',
		'class': ['Rotation'],
		'attribute': {
			'AboutZ': ['hasRZ', float],
			'AboutY': ['hasRY', float],
			'AboutX': ['hasRX', float],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Name': ['hasRotation', ['CoordinateSystem']],
		},
	},

	# {
	# 	'sheetName': 'Bridge Layout Line 1 - General',
	# 	'class': ['Point'],
	# 	'attribute': {
	# 		'Y': ['hasY', float],
	# 		'X': ['hasX', float],
	# 		'Z': ['hasZ', float],
	# 	},
	# 	'foreignKey': {
	# 		'CoordSys': ['hasCoordinateSystem', ['CoordinateSystem']],
	# 	},
	# 	'invForeignKey': {
	# 		'LayoutLine': ['hasPoint', ['BridgeLayout']],
	# 	},
	# },

	# {
	# 	'sheetName': 'Bridge Layout Line 1 - General',
	# 	'class': ['BridgeLayout'],
	# 	'attribute': {
	# 		'LayoutLine': ['ID', str],
	# 	},
	# 	'foreignKey': {
	# 		'CoordSys': ['hasCoordinateSystem', ['CoordinateSystem']],
	# 	},
	# 	'invForeignKey': {
	# 	},
	# },

	# {
	# 	'sheetName': 'Bridge Layout Line 2 - Horiz',
	# 	'class': ['BridgeHorizontalLayoutSegment'],
	# 	'attribute': {
	# 		'SegType': ['hasSegmentType', str],
	# 		'Bearing': ['hasBearing', str],
	# 		'Station': ['hasStation', float],
	# 		'Radius': ['hasRadius', float],
	# 	},
	# 	'foreignKey': {
	# 	},
	# 	'invForeignKey': {
	# 		'LayoutLine': ['hasBridgeLayoutHorizontalSegment', ['BridgeLayout']],
	# 	},
	# },

	# {
	# 	'sheetName': 'Bridge Layout Line 3 - Vertical',
	# 	'class': ['BridgeVerticalLayoutSegment'],
	# 	'attribute': {
	# 		'Station': ['hasStation', float],
	# 		'Grade': ['hasGrade', float],
	# 		'Z': ['hasElevation', float],
	# 		'SegType': ['hasSegmentType', str],
	# 	},
	# 	'foreignKey': {
	# 	},
	# 	'invForeignKey': {
	# 		'LayoutLine': ['hasBridgeLayoutVerticalSegment', ['BridgeLayout']],
	# 	},
	# },


	{
		'sheetName': 'Joint Coordinates',
		'class': ['Point'],
		'attribute': {
			'Y': ['hasY', float],
			'Z': ['hasZ', float],
			'XorR': ['hasX', float],
		},
		'foreignKey': {
			'CoordSys': ['hasCoordinateSystem', ['CoordinateSystem']],
		},
		'invForeignKey': {
			'Joint': ['hasPoint', ['Node']],
		},
	},


	{
		'sheetName': 'Connectivity - Area',
		'class': ['PlanarElement'],
		'attribute': {
			'Area': ['ID', str],
		},
		'foreignKey': {
			'Joint1': ['hasAreaNode', ['Node']],
			'Joint2': ['hasAreaNode', ['Node']],
			'Joint3': ['hasAreaNode', ['Node']],
			'Joint4': ['hasAreaNode', ['Node']],
		},
		'invForeignKey': {
		},
	},


	{
		'sheetName': 'Connectivity - Frame',
		'class': ['FrameElement'],
		'attribute': {
			'Frame': ['ID', str],
		},
		'foreignKey': {
			'JointJ': ['hasNode2', ['Node']],
			'JointI': ['hasNode1', ['Node']],
		},
		'invForeignKey': {
		},
	},

	{
		'sheetName': 'Frame Auto Mesh',
		'class': ['LineAutoMesh'],
		'attribute': {
			'AutoMesh': ['enableAutoMesh', bool],
			'MaxDegrees': ['hasAutoMeshMaxDegree', float],
			'AtJoints': ['hasAutoMeshAtJoint', bool],
			'AtFrames': ['hasAutoMeshAtFrame', bool],
			'MaxLength': ['hasAutoMeshMaxLength', float],
			'NumSegments': ['hasAutoMeshNumberOfSegment', int],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Frame': ['hasLineAutoMesh', ['FrameElement']],
		},
	},

	{
		'sheetName': 'Frame Releases 1 - General',
		'class': ['FrameRelease'],
		'attribute': {
			'M2I': ['hasFrameReleaseRy', bool],
			'M3I': ['hasFrameReleaseRz', bool],
			'TI': ['hasFrameReleaseRx', bool],
			'V3I': ['hasFrameReleaseUz', bool],
			'V2I': ['hasFrameReleaseUy', bool],
			'PI': ['hasFrameReleaseUx', bool],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Frame': ['hasFrameReleaseAtNode1', ['FrameElement']],
		},
	},

	{
		'sheetName': 'Frame Releases 1 - General',
		'class': ['FrameRelease'],
		'attribute': {
			'TJ': ['hasFrameReleaseR1', bool],
			'M3J': ['hasFrameReleaseR3', bool],
			'M2J': ['hasFrameReleaseR2', bool],
			'PJ': ['hasFrameReleaseU1', bool],
			'V2J': ['hasFrameReleaseU2', bool],
			'V3J': ['hasFrameReleaseU3', bool],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Frame': ['hasFrameReleaseAtNode2', ['FrameElement']],
		},
	},

	{
		'sheetName': 'Frame Props 01 - General',
		'class': ['LineSectionAngle','LineUserDefinedSection','LineSectionTShape','LineSectionCShape'],
		'attribute': {
			'SectionName': ['ID', str],
			'Shape': ['classID', str],
			't3': ['hasHeight', float],
			't2': ['hasTopFlangeWidth', float],
			'tf': ['hasFlangeThickness', float],
			'tw': ['hasWebThickness', float],
		},
		'foreignKey': {
			'Material': ['hasMaterial', ['Material']],
		},
		'invForeignKey': {
		},
	},

	{
		'sheetName': 'Frame Section Assignments',
		'class': ['FrameElement'],
		'attribute': {
			'Frame': ['ID', str],
		},
		'foreignKey': {
			'AnalSect': ['hasLineSection', ['LineSectionAngle','LineUserDefinedSection','LineSectionTShape','LineSectionCShape']],
		},
		'invForeignKey': {
		},
	},


	{
		'sheetName': 'Joint Restraint Assignments',
		'class': ['NodeRestraint'],
		'attribute': {
			'R1': ['hasRestraintR1', bool],
			'R2': ['hasRestraintR2', bool],
			'R3': ['hasRestraintR3', bool],
			'U1': ['hasRestraintU1', bool],
			'U3': ['hasRestraintU3', bool],
			'U2': ['hasRestraintU2', bool],
		},
		'foreignKey': {
		},
		'invForeignKey': {
			'Joint': ['hasSupport', ['Node']],
		},
	},

	# {
	# 	'sheetName': 'Lane Definition Data',
	# 	'class': ['LaneSegment'],
	# 	'attribute': {
	# 		'DiscSpan': ['hasLaneDiscretizationSpan', bool],
	# 		'DiscLaneFac': ['hasLaneDiscretizationLaneFactor', float],
	# 		'DiscAlong': ['hasLaneDiscretizationAlongLane', float],
	# 		'DiscLane': ['hasLaneDiscretizationLane', bool],
	# 		'Width': ['hasLaneWidth', float],
	# 		'Station': ['hasLaneStation', float],
	# 		'DiscSpanFac': ['hasLaneDiscretizationSpanFactor', float],
	# 		'DiscAcross': ['hasLaneDiscretizationAcrossLane', float],
	# 		'Offset': ['hasLaneOffset', float],
	# 		'Radius': ['hasLaneRadius', float],
	# 		'LeftType': ['hasLaneLeftType', str],
	# 		'RightType': ['hasLaneRightType', str],
	# 	},
	# 	'foreignKey': {
	# 	},
	# 	'invForeignKey': {
	# 		'Lane': ['hasLaneSegment', ['LaneFromLayout']],
	# 	},
	# },

	# {
	# 	'sheetName': 'Lane Definition Data',
	# 	'class': ['LaneFromLayout'],
	# 	'attribute': {
	# 		'Lane': ['ID', str],
	# 	},
	# 	'foreignKey': {
	# 		'LayoutLine': ['hasLayoutLine', ['BridgeLayout']],
	# 	},
	# 	'invForeignKey': {
	# 	},
	# },

	{
		'sheetName': 'SD 01 - General',
		'class': ['LineUserDefinedSection'],
		'attribute': {
			'SectionName': ['ID', str],
			'DesignType': ['hasDesignType', str],
			'DsgnOrChck':['hasDesignOrCheck', str],
			'IncludeVStr':['hasIncludeVStr', str],	
		},
		'foreignKey': {
		},
		'invForeignKey': {
		},
	},

	{
		'sheetName': 'SD 11 - Shape Plate',
		'class': ['LineUserDefinedSubSectionPlate'],
		'attribute': {
			'ShapeName': ['hasShapeName', str],
			'ZOrder':['hasSubSectionZOrder', int],
			'XCenter':['hasSubSectionXCenter', float],
			'YCenter':['hasSubSectionYCenter', float],
			'Thickness':['hasSubSectionThickness', float],
			'Width':['hasSubSectionWidth', float],
			'Rotation':['hasSubSectionRotation', float],
			'Reinforcing':['hasSubSectionReinforcing', bool],
		},
		'foreignKey': {
			'ShapeMat': ['hasMaterial', ['Material']],
		},
		'invForeignKey': {
			'SectionName': ['hasSubSectionPlate', ['LineUserDefinedSection']],
		},
	},


	{
		'sheetName': 'SD 12 - Shape Solid Rectangle',
		'class': ['LineUserDefinedSubSectionSolidRactangle'],
		'attribute': {
			'ShapeName': ['hasShapeName', str],
			'ZOrder':['hasSubSectionZOrder', int],
			'XCenter':['hasSubSectionXCenter', float],
			'YCenter':['hasSubSectionYCenter', float],
			'Height':['hasSubSectionHeight', float],
			'Width':['hasSubSectionWidth', float],
			'Rotation':['hasSubSectionRotation', float],
			'Reinforcing':['hasSubSectionReinforcing', bool],
		},
		'foreignKey': {
			'ShapeMat': ['hasMaterial', ['Material']],
		},
		'invForeignKey': {
			'SectionName': ['hasSubSectionSolidRactangle', ['LineUserDefinedSection']],
		},
	},
							

]