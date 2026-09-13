using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jogos.API.Migrations
{
    /// <inheritdoc />
    public partial class JogoCategoriaDesenvolvedora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_desenvolvedora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_desenvolvedora", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_jogo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    c_nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    c_preco = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    c_plataforma = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    c_data_lancamento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    c_desenvolvedora_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_jogo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_jogo_tb_desenvolvedora_c_desenvolvedora_id",
                        column: x => x.c_desenvolvedora_id,
                        principalTable: "tb_desenvolvedora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaEntityJogoEntity",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    JogosId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaEntityJogoEntity", x => new { x.CategoriasId, x.JogosId });
                    table.ForeignKey(
                        name: "FK_CategoriaEntityJogoEntity_tb_categoria_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "tb_categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaEntityJogoEntity_tb_jogo_JogosId",
                        column: x => x.JogosId,
                        principalTable: "tb_jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaEntityJogoEntity_JogosId",
                table: "CategoriaEntityJogoEntity",
                column: "JogosId");

            migrationBuilder.CreateIndex(
                name: "IDX_categoria_nome",
                table: "tb_categoria",
                column: "c_nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_desenvolvedora_nome",
                table: "tb_desenvolvedora",
                column: "c_nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_jogo_nome_plataforma",
                table: "tb_jogo",
                columns: new[] { "c_nome", "c_plataforma" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_jogo_c_desenvolvedora_id",
                table: "tb_jogo",
                column: "c_desenvolvedora_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaEntityJogoEntity");

            migrationBuilder.DropTable(
                name: "tb_categoria");

            migrationBuilder.DropTable(
                name: "tb_jogo");

            migrationBuilder.DropTable(
                name: "tb_desenvolvedora");
        }
    }
}
