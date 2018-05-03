using System;
using Porter.Midas;
using System.IO;

namespace midas_wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length==0){
                Console.WriteLine("please specify the input file");
                Midas2Owl("../../data/New_TRB_v2-manual-v2.1.mgt","../../ontology/New_TRB_v2-manual-v2.1_midas.owl");
                Owl2Midas("../../ontology/New_TRB_v2-manual-v2.1_midas.owl","../../data/New_TRB_v2-manual-v2.1_auto.mgt");
                return;
            }
            foreach(var f in args){
                if(!File.Exists(f)){
                    Console.WriteLine("[Error] file not found: {0}.",f);
                    continue;
                }
                var ext=Path.GetExtension(f);
                if(ext==".mgt"||ext==".mct"){
                    var output=f.Replace(".mgt",".owl");
                    Midas2Owl(f,output);
                }else if(ext==".owl"){
                    var output=f.Replace(".owl",".mgt");
                    Owl2Midas(f,output);
                }else{
                    Console.WriteLine("[Error] unrecognized file format: {0}.",f);
                    continue;                    
                }
                
                Console.WriteLine("[Info] file successfully processed: {0}.",f);                
            }
        }

        private static void Owl2Midas(string input,string output)
        {
            var trans=new Owl2Midas();
            trans.GenerateMgtFile(input,output);
        }

        private static void Midas2Owl(string input,string output)
        {
            MidasImporter importer=new MidasImporter();
            var data = importer.Import(input);
           var conv=new Midas2Owl("http://eil.stanford.edu/ontologies/trb#",data);
            conv.ImportOntology("brim", "http://eil.stanford.edu/ontologies/brim#");
            conv.Export(output);
        }
    }
}
