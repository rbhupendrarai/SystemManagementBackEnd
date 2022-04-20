using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemManagement.Data.Migrations
{
    public partial class spFiltersSubModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[spSubModelFilter]
	                                @Sort Varchar(10),
	                                @Page_Size int,
	                                @Page_Limit int
                              As
	                                 SELECT  S.SM_Id,S.SM_Name,S.SM_Discription,S.SM_Feature,S.SM_Price,
                                             M.Mo_Id,M.Mo_Name,
                                             C.CR_Id,C.CR_Name                                       
                                                                                      
                                     FROM SubModel S INNER JOIN Model M ON S.MO_Id=M.MO_Id 
                                                     INNER JOIN Car C ON C.CR_Id=M.CR_Id
                                     WHERE S.IsDeleted='False'

	                                 ORDER by

                                     CASE WHEN  @Sort='ASC' THEN  C.CR_Id END ASC,              
                                     CASE WHEN  @Sort='DESC' THEN C.CR_Id  END DESC,
                                     CASE WHEN  @Sort='ASC' THEN  C.CR_Name END ASC,
                                     CASE WHEN  @Sort='DESC' THEN C.CR_Name  END DESC,                          
                                     CASE WHEN  @Sort='ASC' THEN  M.Mo_Id END ASC,    
                                     CASE WHEN  @Sort='DESC' THEN M.Mo_Id END DESC,
                                     CASE WHEN  @Sort='ASC' THEN  M.Mo_Name END ASC,
                                     CASE WHEN  @Sort='DESC' THEN M.Mo_Name END DESC,                                    
                                     CASE WHEN  @Sort='ASC' THEN S.SM_Id END ASC,         
                                     CASE WHEN  @Sort='DESC' THEN S.SM_Id END DESC,
		                             CASE WHEN  @Sort='ASC' THEN S.SM_Name END ASC,
                                     CASE WHEN  @Sort='DESC' THEN S.SM_Name END DESC,      
                                     CASE WHEN  @Sort='ASC'  THEN S.SM_Discription END ASC,
                                     CASE WHEN  @Sort='DESC' THEN S.SM_Discription END DESC,		
                                     CASE WHEN  @Sort='ASC'  THEN S.SM_Feature END ASC,
                                     CASE WHEN  @Sort='DESC' THEN S.SM_Feature END DESC,	
                                     CASE WHEN  @Sort='ASC'  THEN S.SM_Price END ASC,
                                     CASE WHEN  @Sort='DESC' THEN S.SM_Price END DESC
                                     OFFSET @Page_Limit * (@Page_Size -1) ROWS FETCH NEXT @Page_Limit ROWS ONLY";

                            
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE [dbo].[spSubModelFilter]";
            migrationBuilder.Sql(procedure);
        }
    }
}
