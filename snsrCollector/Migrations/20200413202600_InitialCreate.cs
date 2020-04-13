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
                    ID_Key = table.Column<int>(nullable: false),
                    Ld_type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_logical_type_pkey", x => x.ID_Key);
                },
                comment: "Тип логического прибора - словарь с перечислением типов логических приборов.");

            migrationBuilder.CreateTable(
                name: "model_type",
                columns: table => new
                {
                    ID_Key = table.Column<int>(nullable: false),
                    Model_type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_type_pkey", x => x.ID_Key);
                },
                comment: "Тип модели - словарь с перечислением типов.");

            migrationBuilder.CreateTable(
                name: "object_type_dict",
                columns: table => new
                {
                    ID_Key = table.Column<int>(nullable: false),
                    Object_type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("object_dict_pkey", x => x.ID_Key);
                },
                comment: @"Словарь типов объектов - перечисление типов (Число, коммуникационный объект, строка, т.д.)
");

            migrationBuilder.CreateTable(
                name: "profile_type",
                columns: table => new
                {
                    ID_Key = table.Column<int>(nullable: false),
                    Type_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("profile_type_pkey", x => x.ID_Key);
                });

            migrationBuilder.CreateTable(
                name: "model",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Model_type_fkey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "model_Model_type_fkey_fkey",
                        column: x => x.Model_type_fkey,
                        principalTable: "model_type",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Модель прибора - описание, чертёж прибора. Ближайшее сравнение - классы в ООП. По их лекалу будут создаваться экземпляры, соответствующие физическим приборам, расположенным в реальном мире.");

            migrationBuilder.CreateTable(
                name: "object_dict",
                columns: table => new
                {
                    ID_Key = table.Column<int>(nullable: false),
                    Object_name = table.Column<string>(type: "character varying", nullable: false),
                    Object_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("object_type_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "object_dict_Object_type_fkey",
                        column: x => x.Object_type,
                        principalTable: "object_type_dict",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Словарь объектов - перечисление объектов, которые могут содержаться в приборах. Например, температура, серийный номер, ip адрес.");

            migrationBuilder.CreateTable(
                name: "model_logical_device",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Model_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Ld_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_logical_device_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "model_logical_device_Ld_type_fkey",
                        column: x => x.Ld_type,
                        principalTable: "model_logical_type",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_logical_device_Model_fkey_fkey",
                        column: x => x.Model_fkey,
                        principalTable: "model",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Логический прибор - слой в модели, который содержит в себе профили и объекты. Необходим для идентификации объектов, к примеру, если в приборе есть два микроконтроллера, измеряющие температуру, для идентификации используются разные логические приборы. Будет создано 2 логических прибора, в каждом по объекту \"Температура\"");

            migrationBuilder.CreateTable(
                name: "model_profile",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Model_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Profile_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_profile_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "model_profile_Model_ld_fkey_fkey",
                        column: x => x.Model_ld_fkey,
                        principalTable: "model_logical_device",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_profile_Profile_type_fkey",
                        column: x => x.Profile_type,
                        principalTable: "profile_type",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Профиль содержит информацию для коммуникации с прибором.");

            migrationBuilder.CreateTable(
                name: "model_logical_device_object",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Model_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Model_profile_fkey = table.Column<string>(type: "character varying", nullable: true),
                    Object_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("model_logical_device_object_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "model_logical_device_object_Model_ld_fkey_fkey",
                        column: x => x.Model_ld_fkey,
                        principalTable: "model_logical_device",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_logical_device_object_Model_profile_fkey_fkey",
                        column: x => x.Model_profile_fkey,
                        principalTable: "model_profile",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "model_logical_device_object_Object_id_fkey",
                        column: x => x.Object_id,
                        principalTable: "object_dict",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Объекты логического прибора - ссылки, связывающие профили и логические приборы с словарём объектов");

            migrationBuilder.CreateTable(
                name: "device_logical",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Device_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Model_logical_device = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_logical_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "device_logical_Model_logical_device_fkey",
                        column: x => x.Model_logical_device,
                        principalTable: "model_logical_device",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_logical - связь логического прибора созданного экземпляра с тем прибором, что есть в модели, по которой создано отображение физического прибора.");

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Model_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Main_logical_device = table.Column<string>(type: "character varying", nullable: true),
                    Serial_number = table.Column<string>(type: "character varying", nullable: false),
                    Device_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "device_Device_type_fkey",
                        column: x => x.Device_type,
                        principalTable: "model_type",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_Main_logical_device_fkey",
                        column: x => x.Main_logical_device,
                        principalTable: "device_logical",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_Model_fkey_fkey",
                        column: x => x.Model_fkey,
                        principalTable: "model",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Прибор - экземпляр модели, представляющий и отображающий настоящий где-либо расположенный прибор в реальной жизни");

            migrationBuilder.CreateTable(
                name: "device_profile",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Device_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Model_profile_fkey = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_profile_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "device_profile_Device_ld_fkey_fkey",
                        column: x => x.Device_ld_fkey,
                        principalTable: "device_logical",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_profile_Model_profile_fkey_fkey",
                        column: x => x.Model_profile_fkey,
                        principalTable: "model_profile",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_profile - связь профиля созданного экземпляра модели прибора.");

            migrationBuilder.CreateTable(
                name: "network",
                columns: table => new
                {
                    ID_Key = table.Column<int>(nullable: false),
                    Left_device_id = table.Column<string>(type: "character varying", nullable: false),
                    Right_device_id = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("network_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "network_Left_device_id_fkey",
                        column: x => x.Left_device_id,
                        principalTable: "device",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "network_Right_device_id_fkey",
                        column: x => x.Right_device_id,
                        principalTable: "device",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "network - сеть, граф, для того, чтобы в системе понимать связь между приборами.");

            migrationBuilder.CreateTable(
                name: "device_object",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Device_profile_fkey = table.Column<string>(type: "character varying", nullable: true),
                    Device_ld_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Model_object_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Start_value = table.Column<string>(type: "character varying", nullable: true),
                    Object_dict_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_object_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "device_object_Device_ld_fkey_fkey",
                        column: x => x.Device_ld_fkey,
                        principalTable: "device_logical",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_object_Device_profile_fkey_fkey",
                        column: x => x.Device_profile_fkey,
                        principalTable: "device_profile",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_object_Model_object_fkey_fkey",
                        column: x => x.Model_object_fkey,
                        principalTable: "model_logical_device_object",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "device_object_Object_dict_id_fkey",
                        column: x => x.Object_dict_id,
                        principalTable: "object_dict",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_object - связь объекта созданного экземпляра прибора с тем, что в модели");

            migrationBuilder.CreateTable(
                name: "profile_network",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Left_profile_id = table.Column<string>(type: "character varying", nullable: true),
                    Right_profile_id = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("primary", x => x.ID_Key);
                    table.ForeignKey(
                        name: "left_profile",
                        column: x => x.Left_profile_id,
                        principalTable: "device_profile",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "right_profile",
                        column: x => x.Right_profile_id,
                        principalTable: "device_profile",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "device_object_value",
                columns: table => new
                {
                    ID_Key = table.Column<string>(type: "character varying", nullable: false),
                    Device_object_fkey = table.Column<string>(type: "character varying", nullable: false),
                    Object_value = table.Column<string>(type: "character varying", nullable: false),
                    Receive_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_object_value_pkey", x => x.ID_Key);
                    table.ForeignKey(
                        name: "device_object_value_Device_object_fkey_fkey",
                        column: x => x.Device_object_fkey,
                        principalTable: "device_object",
                        principalColumn: "ID_Key",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "device_object_value - значение объекта в устройстве. Например, значение температуры.");

            migrationBuilder.CreateIndex(
                name: "IX_device_Device_type",
                table: "device",
                column: "Device_type");

            migrationBuilder.CreateIndex(
                name: "IX_device_Main_logical_device",
                table: "device",
                column: "Main_logical_device");

            migrationBuilder.CreateIndex(
                name: "IX_device_Model_fkey",
                table: "device",
                column: "Model_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_logical_Device_fkey",
                table: "device_logical",
                column: "Device_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_logical_Model_logical_device",
                table: "device_logical",
                column: "Model_logical_device");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_Device_ld_fkey",
                table: "device_object",
                column: "Device_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_Device_profile_fkey",
                table: "device_object",
                column: "Device_profile_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_Model_object_fkey",
                table: "device_object",
                column: "Model_object_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_Object_dict_id",
                table: "device_object",
                column: "Object_dict_id");

            migrationBuilder.CreateIndex(
                name: "IX_device_object_value_Device_object_fkey",
                table: "device_object_value",
                column: "Device_object_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_profile_Device_ld_fkey",
                table: "device_profile",
                column: "Device_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_profile_Model_profile_fkey",
                table: "device_profile",
                column: "Model_profile_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_Model_type_fkey",
                table: "model",
                column: "Model_type_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_Ld_type",
                table: "model_logical_device",
                column: "Ld_type");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_Model_fkey",
                table: "model_logical_device",
                column: "Model_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_object_Model_ld_fkey",
                table: "model_logical_device_object",
                column: "Model_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_object_Model_profile_fkey",
                table: "model_logical_device_object",
                column: "Model_profile_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_logical_device_object_Object_id",
                table: "model_logical_device_object",
                column: "Object_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_profile_Model_ld_fkey",
                table: "model_profile",
                column: "Model_ld_fkey");

            migrationBuilder.CreateIndex(
                name: "IX_model_profile_Profile_type",
                table: "model_profile",
                column: "Profile_type");

            migrationBuilder.CreateIndex(
                name: "IX_network_Left_device_id",
                table: "network",
                column: "Left_device_id");

            migrationBuilder.CreateIndex(
                name: "IX_network_Right_device_id",
                table: "network",
                column: "Right_device_id");

            migrationBuilder.CreateIndex(
                name: "IX_object_dict_Object_type",
                table: "object_dict",
                column: "Object_type");

            migrationBuilder.CreateIndex(
                name: "IX_profile_network_Left_profile_id",
                table: "profile_network",
                column: "Left_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_profile_network_Right_profile_id",
                table: "profile_network",
                column: "Right_profile_id");

            migrationBuilder.AddForeignKey(
                name: "device_logical_Device_fkey_fkey",
                table: "device_logical",
                column: "Device_fkey",
                principalTable: "device",
                principalColumn: "ID_Key",
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
