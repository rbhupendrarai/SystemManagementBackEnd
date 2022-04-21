using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemManagement.Data.Migrations
{
    public partial class spSModelFilters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create PROCEDURE[dbo].[spSModelFilter]
                                    @Sort Varchar(10),
	                                @Page_Size int,
                                    @Page_Limit int,
                                    @Search varchar(30)
                              As

                              Select* from(
                                     SELECT S.SM_Id, S.SM_Name, S.SM_Discription, S.SM_Feature, S.SM_Price, S.IsDeleted,
                                             M.Mo_Id, M.Mo_Name,
                                             C.CR_Id, C.CR_Name

                                     FROM SubModel S INNER JOIN Model M ON S.MO_Id= M.MO_Id
                                                     INNER JOIN Car C ON C.CR_Id= M.CR_Id

                                    WHERE(
                                             @Search IS NULL  OR S.SM_Name LIKE '%'+@Search+'%') or
                                             (@Search IS NULL OR  S.SM_Discription LIKE '%'+@Search+'%') or
                                             (@Search IS NULL OR S.SM_Feature LIKE '%'+@Search+'%') or
                                             (@Search IS NULL OR S.SM_Price LIKE '%'+@Search+'%') or
                                             (@Search IS NULL OR M.MO_Name LIKE '%'+@Search+'%') or
                                             (@Search IS NULL OR C.CR_Name LIKE '%'+@Search+'%')
									 	   ) As SM

                                     WHERE SM.IsDeleted = 'False'

                                     ORDER by
                                     CASE WHEN  @Sort = 'ASC' THEN SM.CR_Id END ASC,              
                                     CASE WHEN  @Sort = 'DESC' THEN SM.CR_Id END DESC,
                                     CASE WHEN  @Sort = 'ASC' THEN SM.CR_Name END ASC,
                                     CASE WHEN  @Sort = 'DESC' THEN SM.CR_Name END DESC,                          
                                     CASE WHEN  @Sort = 'ASC' THEN SM.Mo_Id END ASC,    
                                     CASE WHEN  @Sort = 'DESC' THEN SM.Mo_Id END DESC,
                                     CASE WHEN  @Sort = 'ASC' THEN SM.Mo_Name END ASC,
                                     CASE WHEN  @Sort = 'DESC' THEN SM.Mo_Name END DESC,                                    
                                     CASE WHEN  @Sort = 'ASC' THEN SM.SM_Id END ASC,         
                                     CASE WHEN  @Sort = 'DESC' THEN SM.SM_Id END DESC,
		                             CASE WHEN  @Sort = 'ASC' THEN SM.SM_Name END ASC,
                                     CASE WHEN  @Sort = 'DESC' THEN SM.SM_Name END DESC,      
                                     CASE WHEN  @Sort = 'ASC'  THEN SM.SM_Discription END ASC,
                                     CASE WHEN  @Sort = 'DESC' THEN SM.SM_Discription END DESC,		
                                     CASE WHEN  @Sort = 'ASC'  THEN SM.SM_Feature END ASC,
                                     CASE WHEN  @Sort = 'DESC' THEN SM.SM_Feature END DESC,	
                                     CASE WHEN  @Sort = 'ASC'  THEN SM.SM_Price END ASC,
                                     CASE WHEN  @Sort = 'DESC' THEN SM.SM_Price END DESC
                                     OFFSET @Page_Limit * (@Page_Size - 1) ROWS FETCH NEXT @Page_Limit ROWS ONLY";
            migrationBuilder.Sql(procedure);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE [dbo].[spSModelFilter]";
            migrationBuilder.Sql(procedure);
        }
    }
}
