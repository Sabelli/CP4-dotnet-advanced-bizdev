using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jogos.API.Migrations
{
    /// <inheritdoc />
    public partial class JogoPlataformaRefatoracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_jogo_nome_plataforma",
                table: "tb_jogo");

            migrationBuilder.DropColumn(
                name: "c_plataforma",
                table: "tb_jogo");

            migrationBuilder.CreateTable(
                name: "tb_plataforma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_plataforma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JogoEntityPlataformaEntity",
                columns: table => new
                {
                    JogosId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PlataformasId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JogoEntityPlataformaEntity", x => new { x.JogosId, x.PlataformasId });
                    table.ForeignKey(
                        name: "FK_JogoEntityPlataformaEntity_tb_jogo_JogosId",
                        column: x => x.JogosId,
                        principalTable: "tb_jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JogoEntityPlataformaEntity_tb_plataforma_PlataformasId",
                        column: x => x.PlataformasId,
                        principalTable: "tb_plataforma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_jogo_nome",
                table: "tb_jogo",
                column: "c_nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JogoEntityPlataformaEntity_PlataformasId",
                table: "JogoEntityPlataformaEntity",
                column: "PlataformasId");

            migrationBuilder.CreateIndex(
                name: "IDX_plataforma_nome",
                table: "tb_plataforma",
                column: "c_nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JogoEntityPlataformaEntity");

            migrationBuilder.DropTable(
                name: "tb_plataforma");

            migrationBuilder.DropIndex(
                name: "IDX_jogo_nome",
                table: "tb_jogo");

            migrationBuilder.AddColumn<string>(
                name: "c_plataforma",
                table: "tb_jogo",
                type: "NVARCHAR2(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IDX_jogo_nome_plataforma",
                table: "tb_jogo",
                columns: new[] { "c_nome", "c_plataforma" },
                unique: true);
        }
    }
}
