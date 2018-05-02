using System;
using Porter.Midas;

namespace midas_wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var trans=new Owl2Midas();
            trans.GenerateMgtFile("../../ontology/New_TRB_v2-manual-v2.1_midas.owl","../../data/New_TRB_v2-manual-v2.1_auto.mgt");
           
            MidasImporter importer=new MidasImporter();
            var data = importer.Import("../../data/New_TRB_v2-manual-v2.1.mgt");
           var conv=new Midas2Owl("http://eil.stanford.edu/ontologies/trb#",data);
            conv.ImportOntology("brim", "http://eil.stanford.edu/ontologies/brim#");
            conv.Export(@"../../ontology/New_TRB_v2-manual-v2.1_midas.owl");
            Console.WriteLine("Conversion finished...");
        }
    }
}
