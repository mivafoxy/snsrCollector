using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace snsrCollector.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "model_logical_type",
                columns: table => new
                {
                    id_key = table.Column<int>(nullable: false),
                    ld_type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_logical_type_pkey", x => x.id_key);
                },
                comment: "Тип логического прибора - словарь с перечислением типов логических приборов.");

            migrationBuilder.CreateTable(
                name: "model_type",
                columns: table => new
                {
                    id_key = table.Column<int>(nullable: false),
                    model_type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_type_pkey", x => x.id_key);
                },
                comment: "Тип модели - словарь с перечислением типов.");

            migrationBuilder.CreateTable(
                name: "object_type_dict",
                columns: table => new
                {
                    id_key = table.Column<int>(nullable: false),
                    object_type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("object_dict_pkey", x => x.id_key);
                },
                comment: @"Словарь типов объектов - перечисление типов (Число, коммуникационный объект, строка, т.д.)
");

            migrationBuilder.CreateTable(
                name: "profile_type",
                columns: table => new
                {
                    id_key = table.Column<int>(nullable: false),
                    type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("profile_type_pkey", x => x.id_key);
                });

            migrationBuilder.CreateTable(
                name: "model",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    model_type_fkey = table.Column<int>(nullable: false),
                    model_name = table.Column<string>(type: "character varying", nullable: false, defaultValueSql: "'UNKNOWN'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "model_Model_type_fkey_fkey",
                        column: x => x.model_type_fkey,
                        principalTable: "model_type",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Модель прибора - описание, чертёж прибора. Ближайшее сравнение - классы в ООП. По их лекалу будут создаваться экземпляры, соответствующие физическим приборам, расположенным в реальном мире.");

            migrationBuilder.CreateTable(
                name: "object_dict",
                columns: table => new
                {
                    id_key = table.Column<int>(nullable: false),
                    object_name = table.Column<string>(type: "character varying", nullable: false),
                    object_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("object_type_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "object_dict_Object_type_fkey",
                        column: x => x.object_type,
                        principalTable: "object_type_dict",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Словарь объектов - перечисление объектов, которые могут содержаться в приборах. Например, температура, серийный номер, ip адрес.");

            migrationBuilder.CreateTable(
                name: "model_logical_device",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    model_fkey = table.Column<string>(type: "character varying", nullable: false),
                    ld_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_logical_device_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "model_logical_device_Ld_type_fkey",
                        column: x => x.ld_type,
                        principalTable: "model_logical_type",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_logical_device_Model_fkey_fkey",
                        column: x => x.model_fkey,
                        principalTable: "model",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Логический прибор - слой в модели, который содержит в себе профили и объекты. Необходим для идентификации объектов, к примеру, если в приборе есть два микроконтроллера, измеряющие температуру, для идентификации используются разные логические приборы. Будет создано 2 логических прибора, в каждом по объекту \"Температура\"");

            migrationBuilder.CreateTable(
                name: "model_profile",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    model_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    profile_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_profile_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "model_profile_Model_ld_fkey_fkey",
                        column: x => x.model_ld_fkey,
                        principalTable: "model_logical_device",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_profile_Profile_type_fkey",
                        column: x => x.profile_type,
                        principalTable: "profile_type",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Профиль содержит информацию для коммуникации с прибором.");

            migrationBuilder.CreateTable(
                name: "model_logical_device_object",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    model_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    model_profile_fkey = table.Column<string>(type: "character varying", nullable: true),
                    object_id = table.Column<int>(nullable: false),
                    is_editable = table.Column<bool>(nullable: false),
                    is_initable = table.Column<bool>(nullable: false),
                    is_shown = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_logical_device_object_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "model_logical_device_object_Model_ld_fkey_fkey",
                        column: x => x.model_ld_fkey,
                        principalTable: "model_logical_device",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_logical_device_object_Model_profile_fkey_fkey",
                        column: x => x.model_profile_fkey,
                        principalTable: "model_profile",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_logical_device_object_Object_id_fkey",
                        column: x => x.object_id,
                        principalTable: "object_dict",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Объекты логического прибора - ссылки, связывающие профили и логические приборы с словарём объектов");

            migrationBuilder.CreateTable(
                name: "device_logical",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    device_fkey = table.Column<string>(type: "character varying", nullable: false),
                    model_logical_device = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_logical_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "device_logical_Model_logical_device_fkey",
                        column: x => x.model_logical_device,
                        principalTable: "model_logical_device",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_logical - связь логического прибора созданного экземпляра с тем прибором, что есть в модели, по которой создано отображение физического прибора.");

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    model_fkey = table.Column<string>(type: "character varying", nullable: false),
                    main_logical_device = table.Column<string>(type: "character varying", nullable: true),
                    serial_number = table.Column<string>(type: "character varying", nullable: false),
                    device_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "device_Device_type_fkey",
                        column: x => x.device_type,
                        principalTable: "model_type",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_Main_logical_device_fkey",
                        column: x => x.main_logical_device,
                        principalTable: "device_logical",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_Model_fkey_fkey",
                        column: x => x.model_fkey,
                        principalTable: "model",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Прибор - экземпляр модели, представляющий и отображающий настоящий где-либо расположенный прибор в реальной жизни");

            migrationBuilder.CreateTable(
                name: "device_profile",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    device_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    model_profile_fkey = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_profile_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "device_profile_Device_ld_fkey_fkey",
                        column: x => x.device_ld_fkey,
                        principalTable: "device_logical",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_profile_Model_profile_fkey_fkey",
                        column: x => x.model_profile_fkey,
                        principalTable: "model_profile",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_profile - связь профиля созданного экземпляра модели прибора.");

            migrationBuilder.CreateTable(
                name: "network",
                columns: table => new
                {
                    id_key = table.Column<int>(nullable: false),
                    parent_device_id = table.Column<string>(type: "character varying", nullable: false),
                    child_device_id = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("network_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "network_Right_device_id_fkey",
                        column: x => x.child_device_id,
                        principalTable: "device",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "network_Left_device_id_fkey",
                        column: x => x.parent_device_id,
                        principalTable: "device",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "network - сеть, граф, для того, чтобы в системе понимать связь между приборами.");

            migrationBuilder.CreateTable(
                name: "device_object",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    device_profile_fkey = table.Column<string>(type: "character varying", nullable: true),
                    device_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    model_object_fkey = table.Column<string>(type: "character varying", nullable: false),
                    start_value = table.Column<string>(type: "character varying", nullable: true),
                    object_dict_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_object_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "device_object_Device_ld_fkey_fkey",
                        column: x => x.device_ld_fkey,
                        principalTable: "device_logical",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_object_Device_profile_fkey_fkey",
                        column: x => x.device_profile_fkey,
                        principalTable: "device_profile",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_object_Model_object_fkey_fkey",
                        column: x => x.model_object_fkey,
                        principalTable: "model_logical_device_object",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_object_Object_dict_id_fkey",
                        column: x => x.object_dict_id,
                        principalTable: "object_dict",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_object - связь объекта созданного экземпляра прибора с тем, что в модели");

            migrationBuilder.CreateTable(
                name: "profile_network",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    parent_profile_id = table.Column<string>(type: "character varying", nullable: true),
                    child_profile_id = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("primary", x => x.id_key);
                    table.ForeignKey(
                        name: "right_profile",
                        column: x => x.child_profile_id,
                        principalTable: "device_profile",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "left_profile",
                        column: x => x.parent_profile_id,
                        principalTable: "device_profile",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "device_object_value",
                columns: table => new
                {
                    id_key = table.Column<string>(type: "character varying", nullable: false),
                    device_object_fkey = table.Column<string>(type: "character varying", nullable: false),
                    object_value = table.Column<string>(type: "character varying", nullable: false),
                    receive_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_object_value_pkey", x => x.id_key);
                    table.ForeignKey(
                        name: "device_object_value_Device_object_fkey_fkey",
                        column: x => x.device_object_fkey,
                        principalTable: "device_object",
                        principalColumn: "id_key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_object_value - значение объекта в устройстве. Например, значение температуры.");

            migrationBuilder.CreateIndex(
                name: "IX_device_device_type",
                table: "device",
                column: "device_type");

            migrationBuilder.CreateIndex(
                name: "IX_device_main_logical_device",
                table: "device",
                column: "main_logical_device");

            migrationBuilder.CreateIndex(
                name: "IX_device_model_fkey",
                table: "device",
                column: "model_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_logical_device_fkey",
                table: "device_logical",
                column: "device_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_logical_model_logical_device",
                table: "device_logical",
                column: "model_logical_device");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_device_ld_fkey",
                table: "device_object",
                column: "device_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_device_profile_fkey",
                table: "device_object",
                column: "device_profile_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_model_object_fkey",
                table: "device_object",
                column: "model_object_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_object_dict_id",
                table: "device_object",
                column: "object_dict_id");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_value_device_object_fkey",
                table: "device_object_value",
                column: "device_object_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_profile_device_ld_fkey",
                table: "device_profile",
                column: "device_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_profile_model_profile_fkey",
                table: "device_profile",
                column: "model_profile_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_model_type_fkey",
                table: "model",
                column: "model_type_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_ld_type",
                table: "model_logical_device",
                column: "ld_type");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_model_fkey",
                table: "model_logical_device",
                column: "model_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_object_model_ld_fkey",
                table: "model_logical_device_object",
                column: "model_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_object_model_profile_fkey",
                table: "model_logical_device_object",
                column: "model_profile_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_object_object_id",
                table: "model_logical_device_object",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_profile_model_ld_fkey",
                table: "model_profile",
                column: "model_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_profile_profile_type",
                table: "model_profile",
                column: "profile_type");

            migrationBuilder.CreateIndex(
                name: "IX_network_child_device_id",
                table: "network",
                column: "child_device_id");

            migrationBuilder.CreateIndex(
                name: "IX_network_parent_device_id",
                table: "network",
                column: "parent_device_id");

            migrationBuilder.CreateIndex(
                name: "IX_object_dict_object_type",
                table: "object_dict",
                column: "object_type");

            migrationBuilder.CreateIndex(
                name: "IX_profile_network_child_profile_id",
                table: "profile_network",
                column: "child_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_profile_network_parent_profile_id",
                table: "profile_network",
                column: "parent_profile_id");

            migrationBuilder.AddForeignKey(
                name: "device_logical_Device_fkey_fkey",
                table: "device_logical",
                column: "device_fkey",
                principalTable: "device",
                principalColumn: "id_key",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "device_Device_type_fkey",
                table: "device");

            migrationBuilder.DropForeignKey(
                name: "model_Model_type_fkey_fkey",
                table: "model");

            migrationBuilder.DropForeignKey(
                name: "device_Main_logical_device_fkey",
                table: "device");

            migrationBuilder.DropTable(
                name: "device_object_value");

            migrationBuilder.DropTable(
                name: "network");

            migrationBuilder.DropTable(
                name: "profile_network");

            migrationBuilder.DropTable(
                name: "device_object");

            migrationBuilder.DropTable(
                name: "device_profile");

            migrationBuilder.DropTable(
                name: "model_logical_device_object");

            migrationBuilder.DropTable(
                name: "model_profile");

            migrationBuilder.DropTable(
                name: "object_dict");

            migrationBuilder.DropTable(
                name: "profile_type");

            migrationBuilder.DropTable(
                name: "object_type_dict");

            migrationBuilder.DropTable(
                name: "model_type");

            migrationBuilder.DropTable(
                name: "device_logical");

            migrationBuilder.DropTable(
                name: "device");

            migrationBuilder.DropTable(
                name: "model_logical_device");

            migrationBuilder.DropTable(
                name: "model_logical_type");

            migrationBuilder.DropTable(
                name: "model");
        }
    }
}
