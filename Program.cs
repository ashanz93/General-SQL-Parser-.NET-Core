using System;
using gudusoft.gsqlparser;

namespace GeneralSQLParserPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            TGSqlParser sqlParser = new TGSqlParser(EDbVendor.dbvoracle);
            sqlParser.sqltext = Queries.CreateViewQuery;
            int ret = sqlParser.parse();
            string errMessage = sqlParser.Errormessage;
            if(ret == 0)
            {
                for(int i = 0; i < sqlParser.sqlstatements.size(); i++)
                {
                    MetadataAnalysis.MetadataAnalysis.AnalyzeStmt(sqlParser.sqlstatements[i]);
                    Console.WriteLine();
                }
            }
        }
    }
}
