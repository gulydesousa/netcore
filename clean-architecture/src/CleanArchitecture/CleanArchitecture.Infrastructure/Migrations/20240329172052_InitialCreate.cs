using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    apellido = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vin = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    modelo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    direccion_pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion_departamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion_provincia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion_ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion_calle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    precio_monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    precio_tipo_moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mantenimiento_monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    mantenimiento_tipo_moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_ultimo_alquiler = table.Column<DateTime>(type: "datetime2", nullable: false),
                    accesorios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    version = table.Column<long>(type: "bigint", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehiculos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Alquileres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehiculo_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    duracion_start = table.Column<DateOnly>(type: "date", nullable: true),
                    duracion_end = table.Column<DateOnly>(type: "date", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    precio_por_periodo_monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    precio_por_periodo_tipo_moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    precio_mantenimiento_monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    precio_mantenimiento_tipo_moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    precio_accesorios_monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    precio_accesorios_tipo_moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    precio_total_monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    precio_total_tipo_moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_confirmacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_denegacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_completado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_cancelacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alquileres", x => x.id);
                    table.ForeignKey(
                        name: "fk_alquileres_user_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_alquileres_vehiculo_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "Vehiculos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehiculo_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    alquiler_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comentario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviews_alquileres_alquiler_id",
                        column: x => x.alquiler_id,
                        principalTable: "Alquileres",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_reviews_user_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_reviews_vehiculo_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "Vehiculos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_alquileres_user_id",
                table: "Alquileres",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_alquileres_vehiculo_id",
                table: "Alquileres",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_alquiler_id",
                table: "Reviews",
                column: "alquiler_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_user_id",
                table: "Reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_vehiculo_id",
                table: "Reviews",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "Users",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Alquileres");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehiculos");
        }
    }
}
