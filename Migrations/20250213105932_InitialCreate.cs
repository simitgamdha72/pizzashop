using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pizzashop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assign_table",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    table_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assign_table_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state_id = table.Column<int>(type: "integer", nullable: false),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("city_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("country_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    coming_since = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    average_bill = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    max_order = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    visits = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customer_history_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    total_order = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customers_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "invoice",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    table_id = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    order_modified = table.Column<int>(type: "integer", nullable: false),
                    ordertax_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("invoice_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "itemmodifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    items_id = table.Column<int>(type: "integer", nullable: false),
                    modifiers_id = table.Column<int>(type: "integer", nullable: false),
                    minimumselection = table.Column<int>(type: "integer", nullable: false),
                    maximumselection = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("itemmodifier_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menu_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    item_type = table.Column<int>(type: "integer", nullable: false),
                    rate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    available = table.Column<bool>(type: "boolean", nullable: false),
                    default_tax = table.Column<bool>(type: "boolean", nullable: false),
                    taxpercentage = table.Column<decimal>(type: "numeric", nullable: true),
                    short_code = table.Column<int>(type: "integer", nullable: true),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<byte[]>(type: "bytea", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("menu_items_pkey", x => x.id);
                    table.ForeignKey(
                        name: "menu_items_category_id_fkey",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "menu_modifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    modifier_group_id = table.Column<int>(type: "integer", nullable: false),
                    unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("menu_modifier_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "modified_order_detail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    orders_item_details_id = table.Column<int>(type: "integer", nullable: false),
                    modified_name = table.Column<int>(type: "integer", nullable: false),
                    modifiertax = table.Column<decimal>(type: "numeric", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    modifierquantity = table.Column<int>(type: "integer", nullable: true),
                    modifier_rate = table.Column<decimal>(type: "numeric", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("modified_order_detail_pkey", x => x.id);
                    table.ForeignKey(
                        name: "modified_order_detail_modified_name_fkey",
                        column: x => x.modified_name,
                        principalTable: "menu_modifier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "modifiers_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("modifiers_group_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_tax",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    tax_id = table.Column<int>(type: "integer", nullable: false),
                    orders_item_details_id = table.Column<int>(type: "integer", nullable: false),
                    modified_order_detail_id = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_tax_pkey", x => x.id);
                    table.ForeignKey(
                        name: "order_tax_modified_order_detail_id_fkey",
                        column: x => x.modified_order_detail_id,
                        principalTable: "modified_order_detail",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    items_ordered = table.Column<int>(type: "integer", nullable: false),
                    order_type = table.Column<int>(type: "integer", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: true),
                    payment_id = table.Column<int>(type: "integer", nullable: false),
                    rating_id = table.Column<int>(type: "integer", nullable: true),
                    special_instruction = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    comment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_pkey", x => x.id);
                    table.ForeignKey(
                        name: "orders_customer_id_fkey",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ordersitemdetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    items_id = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    itemtax = table.Column<decimal>(type: "numeric", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    instruction = table.Column<string>(type: "text", nullable: true),
                    itemquantity = table.Column<int>(type: "integer", nullable: true),
                    itemrate = table.Column<decimal>(type: "numeric", nullable: true),
                    itemready = table.Column<int>(type: "integer", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ordersitemdetails_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ordersitemdetails_items_id_fkey",
                        column: x => x.items_id,
                        principalTable: "menu_items",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "ordersitemdetails_order_id_fkey",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tax_id = table.Column<int>(type: "integer", nullable: false),
                    invoice_id = table.Column<int>(type: "integer", nullable: false),
                    payment_mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("payments_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_invoice",
                        column: x => x.invoice_id,
                        principalTable: "invoice",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_payments_order_tax",
                        column: x => x.tax_id,
                        principalTable: "order_tax",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("permissions_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    food = table.Column<int>(type: "integer", nullable: false),
                    service = table.Column<int>(type: "integer", nullable: false),
                    ambience = table.Column<int>(type: "integer", nullable: false),
                    comments = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ratings_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ratings_order_id_fkey",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying", nullable: false),
                    password = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<int>(type: "integer", nullable: true),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    zipcode = table.Column<int>(type: "integer", nullable: true),
                    image = table.Column<byte[]>(type: "bytea", nullable: true),
                    role_id = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_users_createdby",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_users_modifiedby",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_users_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "roleswisepermissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    can_view = table.Column<bool>(type: "boolean", nullable: false),
                    can_add_edit = table.Column<bool>(type: "boolean", nullable: false),
                    can_delete = table.Column<bool>(type: "boolean", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("roleswisepermissions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "roleswisepermissions_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "roleswisepermissions_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "roleswisepermissions_permission_id_fkey",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "roleswisepermissions_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sections",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sections_pkey", x => x.id);
                    table.ForeignKey(
                        name: "sections_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "sections_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "state",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("state_pkey", x => x.id);
                    table.ForeignKey(
                        name: "state_country_id_fkey",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "state_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "state_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "taxes_fees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    tax_amount = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    is_enable = table.Column<bool>(type: "boolean", nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("taxes_fees_pkey", x => x.id);
                    table.ForeignKey(
                        name: "taxes_fees_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "taxes_fees_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "unit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    shortcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("unit_pkey", x => x.id);
                    table.ForeignKey(
                        name: "unit_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "unit_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "tables",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tables_pkey", x => x.id);
                    table.ForeignKey(
                        name: "tables_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "tables_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "tables_section_id_fkey",
                        column: x => x.section_id,
                        principalTable: "sections",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    no_of_people = table.Column<decimal>(type: "numeric(2)", precision: 2, nullable: false),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tokens_pkey", x => x.id);
                    table.ForeignKey(
                        name: "tokens_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "tokens_customer_id_fkey",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tokens_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "tokens_section_id_fkey",
                        column: x => x.section_id,
                        principalTable: "sections",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tableorder",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    table_id = table.Column<int>(type: "integer", nullable: false),
                    isavailable = table.Column<bool>(type: "boolean", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tableorder_pkey", x => x.id);
                    table.ForeignKey(
                        name: "tableorder_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "tableorder_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "tableorder_order_id_fkey",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tableorder_table_id_fkey",
                        column: x => x.table_id,
                        principalTable: "tables",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "waiting_list",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token_no = table.Column<int>(type: "integer", nullable: false),
                    waiting_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("waiting_list_pkey", x => x.id);
                    table.ForeignKey(
                        name: "waiting_list_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "waiting_list_customer_id_fkey",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "waiting_list_modified_by_fkey",
                        column: x => x.modified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "waiting_list_token_no_fkey",
                        column: x => x.token_no,
                        principalTable: "tokens",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_assign_table_created_by",
                table: "assign_table",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_assign_table_customer_id",
                table: "assign_table",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_assign_table_modified_by",
                table: "assign_table",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_assign_table_section_id",
                table: "assign_table",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_assign_table_table_id",
                table: "assign_table",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "category_category_key",
                table: "category",
                column: "category",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_created_by",
                table: "category",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_category_modified_by",
                table: "category",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_city_created_by",
                table: "city",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_city_modified_by",
                table: "city",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_city_state_id",
                table: "city",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_country_created_by",
                table: "country",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_country_modified_by",
                table: "country",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_customer_history_created_by",
                table: "customer_history",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_customer_history_customer_id",
                table: "customer_history",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_history_modified_by",
                table: "customer_history",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_customers_created_by",
                table: "customers",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_customers_modified_by",
                table: "customers",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_created_by",
                table: "invoice",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_modified_by",
                table: "invoice",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_order_id",
                table: "invoice",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_order_modified",
                table: "invoice",
                column: "order_modified");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_ordertax_id",
                table: "invoice",
                column: "ordertax_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_section_id",
                table: "invoice",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_table_id",
                table: "invoice",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "IX_itemmodifier_created_by",
                table: "itemmodifier",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_itemmodifier_items_id",
                table: "itemmodifier",
                column: "items_id");

            migrationBuilder.CreateIndex(
                name: "IX_itemmodifier_modified_by",
                table: "itemmodifier",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_itemmodifier_modifiers_id",
                table: "itemmodifier",
                column: "modifiers_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_items_category_id",
                table: "menu_items",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_items_created_by",
                table: "menu_items",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_menu_items_modified_by",
                table: "menu_items",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_menu_items_unit_id",
                table: "menu_items",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "menu_items_name_key",
                table: "menu_items",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_menu_modifier_created_by",
                table: "menu_modifier",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_menu_modifier_modified_by",
                table: "menu_modifier",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_menu_modifier_modifier_group_id",
                table: "menu_modifier",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_modifier_unit_id",
                table: "menu_modifier",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_modified_order_detail_created_by",
                table: "modified_order_detail",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_modified_order_detail_modified_by",
                table: "modified_order_detail",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_modified_order_detail_modified_name",
                table: "modified_order_detail",
                column: "modified_name");

            migrationBuilder.CreateIndex(
                name: "IX_modified_order_detail_orders_item_details_id",
                table: "modified_order_detail",
                column: "orders_item_details_id");

            migrationBuilder.CreateIndex(
                name: "IX_modifiers_group_created_by",
                table: "modifiers_group",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_modifiers_group_modified_by",
                table: "modifiers_group",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_order_tax_created_by",
                table: "order_tax",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_order_tax_modified_by",
                table: "order_tax",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_order_tax_modified_order_detail_id",
                table: "order_tax",
                column: "modified_order_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_tax_order_id",
                table: "order_tax",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_tax_orders_item_details_id",
                table: "order_tax",
                column: "orders_item_details_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_tax_tax_id",
                table: "order_tax",
                column: "tax_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_created_by",
                table: "orders",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_modified_by",
                table: "orders",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_orders_payment_id",
                table: "orders",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_rating_id",
                table: "orders",
                column: "rating_id");

            migrationBuilder.CreateIndex(
                name: "IX_ordersitemdetails_created_by",
                table: "ordersitemdetails",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ordersitemdetails_items_id",
                table: "ordersitemdetails",
                column: "items_id");

            migrationBuilder.CreateIndex(
                name: "IX_ordersitemdetails_modified_by",
                table: "ordersitemdetails",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_ordersitemdetails_order_id",
                table: "ordersitemdetails",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_created_by",
                table: "payments",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_payments_invoice_id",
                table: "payments",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_modified_by",
                table: "payments",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_payments_tax_id",
                table: "payments",
                column: "tax_id");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_created_by",
                table: "permissions",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_modified_by",
                table: "permissions",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_created_by",
                table: "ratings",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_modified_by",
                table: "ratings",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_order_id",
                table: "ratings",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_created_by",
                table: "role",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_role_modified_by",
                table: "role",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "role_role_key",
                table: "role",
                column: "role",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roleswisepermissions_created_by",
                table: "roleswisepermissions",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_roleswisepermissions_modified_by",
                table: "roleswisepermissions",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_roleswisepermissions_permission_id",
                table: "roleswisepermissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_roleswisepermissions_role_id",
                table: "roleswisepermissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_sections_created_by",
                table: "sections",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_sections_modified_by",
                table: "sections",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_state_country_id",
                table: "state",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_state_created_by",
                table: "state",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_state_modified_by",
                table: "state",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_tableorder_created_by",
                table: "tableorder",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_tableorder_modified_by",
                table: "tableorder",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_tableorder_order_id",
                table: "tableorder",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tableorder_table_id",
                table: "tableorder",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "IX_tables_created_by",
                table: "tables",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_tables_modified_by",
                table: "tables",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_tables_section_id",
                table: "tables",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_taxes_fees_created_by",
                table: "taxes_fees",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_taxes_fees_modified_by",
                table: "taxes_fees",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "taxes_fees_name_key",
                table: "taxes_fees",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tokens_created_by",
                table: "tokens",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_customer_id",
                table: "tokens",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_modified_by",
                table: "tokens",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_section_id",
                table: "tokens",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_unit_created_by",
                table: "unit",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_unit_modified_by",
                table: "unit",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_users_created_by",
                table: "users",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_users_modified_by",
                table: "users",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_waiting_list_created_by",
                table: "waiting_list",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_waiting_list_customer_id",
                table: "waiting_list",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_waiting_list_modified_by",
                table: "waiting_list",
                column: "modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_waiting_list_token_no",
                table: "waiting_list",
                column: "token_no");

            migrationBuilder.AddForeignKey(
                name: "assign_table_created_by_fkey",
                table: "assign_table",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "assign_table_modified_by_fkey",
                table: "assign_table",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "assign_table_customer_id_fkey",
                table: "assign_table",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "assign_table_section_id_fkey",
                table: "assign_table",
                column: "section_id",
                principalTable: "sections",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "assign_table_table_id_fkey",
                table: "assign_table",
                column: "table_id",
                principalTable: "tables",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "category_created_by_fkey",
                table: "category",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "category_modified_by_fkey",
                table: "category",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "city_created_by_fkey",
                table: "city",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "city_modified_by_fkey",
                table: "city",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "city_state_id_fkey",
                table: "city",
                column: "state_id",
                principalTable: "state",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "country_created_by_fkey",
                table: "country",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "country_modified_by_fkey",
                table: "country",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "customer_history_created_by_fkey",
                table: "customer_history",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "customer_history_modified_by_fkey",
                table: "customer_history",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "customer_history_customer_id_fkey",
                table: "customer_history",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "customers_created_by_fkey",
                table: "customers",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "customers_modified_by_fkey",
                table: "customers",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "invoice_created_by_fkey",
                table: "invoice",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "invoice_modified_by_fkey",
                table: "invoice",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "invoice_order_id_fkey",
                table: "invoice",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "invoice_order_modified_fkey",
                table: "invoice",
                column: "order_modified",
                principalTable: "modified_order_detail",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "invoice_ordertax_id_fkey",
                table: "invoice",
                column: "ordertax_id",
                principalTable: "order_tax",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "invoice_section_id_fkey",
                table: "invoice",
                column: "section_id",
                principalTable: "sections",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "invoice_table_id_fkey",
                table: "invoice",
                column: "table_id",
                principalTable: "tables",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "itemmodifier_created_by_fkey",
                table: "itemmodifier",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "itemmodifier_modified_by_fkey",
                table: "itemmodifier",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "itemmodifier_items_id_fkey",
                table: "itemmodifier",
                column: "items_id",
                principalTable: "menu_items",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "itemmodifier_modifiers_id_fkey",
                table: "itemmodifier",
                column: "modifiers_id",
                principalTable: "menu_modifier",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "menu_items_created_by_fkey",
                table: "menu_items",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "menu_items_modified_by_fkey",
                table: "menu_items",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "menu_items_unit_id_fkey",
                table: "menu_items",
                column: "unit_id",
                principalTable: "unit",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "menu_modifier_created_by_fkey",
                table: "menu_modifier",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "menu_modifier_modified_by_fkey",
                table: "menu_modifier",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "menu_modifier_modifier_group_id_fkey",
                table: "menu_modifier",
                column: "modifier_group_id",
                principalTable: "modifiers_group",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "menu_modifier_unit_id_fkey",
                table: "menu_modifier",
                column: "unit_id",
                principalTable: "unit",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "modified_order_detail_created_by_fkey",
                table: "modified_order_detail",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "modified_order_detail_modified_by_fkey",
                table: "modified_order_detail",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "modified_order_detail_orders_item_details_id_fkey",
                table: "modified_order_detail",
                column: "orders_item_details_id",
                principalTable: "ordersitemdetails",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "modifiers_group_created_by_fkey",
                table: "modifiers_group",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "modifiers_group_modified_by_fkey",
                table: "modifiers_group",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "order_tax_created_by_fkey",
                table: "order_tax",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "order_tax_modified_by_fkey",
                table: "order_tax",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "order_tax_order_id_fkey",
                table: "order_tax",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "order_tax_orders_item_details_id_fkey",
                table: "order_tax",
                column: "orders_item_details_id",
                principalTable: "ordersitemdetails",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "order_tax_tax_id_fkey",
                table: "order_tax",
                column: "tax_id",
                principalTable: "taxes_fees",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_ratings",
                table: "orders",
                column: "rating_id",
                principalTable: "ratings",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "orders_created_by_fkey",
                table: "orders",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "orders_modified_by_fkey",
                table: "orders",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "orders_payment_id_fkey",
                table: "orders",
                column: "payment_id",
                principalTable: "payments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "ordersitemdetails_created_by_fkey",
                table: "ordersitemdetails",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "ordersitemdetails_modified_by_fkey",
                table: "ordersitemdetails",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "payments_created_by_fkey",
                table: "payments",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "payments_modified_by_fkey",
                table: "payments",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "permissions_created_by_fkey",
                table: "permissions",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "permissions_modified_by_fkey",
                table: "permissions",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "ratings_created_by_fkey",
                table: "ratings",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "ratings_modified_by_fkey",
                table: "ratings",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "role_created_by_fkey",
                table: "role",
                column: "created_by",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "role_modified_by_fkey",
                table: "role",
                column: "modified_by",
                principalTable: "users",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "category_created_by_fkey",
                table: "category");

            migrationBuilder.DropForeignKey(
                name: "category_modified_by_fkey",
                table: "category");

            migrationBuilder.DropForeignKey(
                name: "customers_created_by_fkey",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "customers_modified_by_fkey",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "invoice_created_by_fkey",
                table: "invoice");

            migrationBuilder.DropForeignKey(
                name: "invoice_modified_by_fkey",
                table: "invoice");

            migrationBuilder.DropForeignKey(
                name: "menu_items_created_by_fkey",
                table: "menu_items");

            migrationBuilder.DropForeignKey(
                name: "menu_items_modified_by_fkey",
                table: "menu_items");

            migrationBuilder.DropForeignKey(
                name: "menu_modifier_created_by_fkey",
                table: "menu_modifier");

            migrationBuilder.DropForeignKey(
                name: "menu_modifier_modified_by_fkey",
                table: "menu_modifier");

            migrationBuilder.DropForeignKey(
                name: "modified_order_detail_created_by_fkey",
                table: "modified_order_detail");

            migrationBuilder.DropForeignKey(
                name: "modified_order_detail_modified_by_fkey",
                table: "modified_order_detail");

            migrationBuilder.DropForeignKey(
                name: "modifiers_group_created_by_fkey",
                table: "modifiers_group");

            migrationBuilder.DropForeignKey(
                name: "modifiers_group_modified_by_fkey",
                table: "modifiers_group");

            migrationBuilder.DropForeignKey(
                name: "order_tax_created_by_fkey",
                table: "order_tax");

            migrationBuilder.DropForeignKey(
                name: "order_tax_modified_by_fkey",
                table: "order_tax");

            migrationBuilder.DropForeignKey(
                name: "orders_created_by_fkey",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "orders_modified_by_fkey",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "ordersitemdetails_created_by_fkey",
                table: "ordersitemdetails");

            migrationBuilder.DropForeignKey(
                name: "ordersitemdetails_modified_by_fkey",
                table: "ordersitemdetails");

            migrationBuilder.DropForeignKey(
                name: "payments_created_by_fkey",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "payments_modified_by_fkey",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "ratings_created_by_fkey",
                table: "ratings");

            migrationBuilder.DropForeignKey(
                name: "ratings_modified_by_fkey",
                table: "ratings");

            migrationBuilder.DropForeignKey(
                name: "role_created_by_fkey",
                table: "role");

            migrationBuilder.DropForeignKey(
                name: "role_modified_by_fkey",
                table: "role");

            migrationBuilder.DropForeignKey(
                name: "sections_created_by_fkey",
                table: "sections");

            migrationBuilder.DropForeignKey(
                name: "sections_modified_by_fkey",
                table: "sections");

            migrationBuilder.DropForeignKey(
                name: "tables_created_by_fkey",
                table: "tables");

            migrationBuilder.DropForeignKey(
                name: "tables_modified_by_fkey",
                table: "tables");

            migrationBuilder.DropForeignKey(
                name: "taxes_fees_created_by_fkey",
                table: "taxes_fees");

            migrationBuilder.DropForeignKey(
                name: "taxes_fees_modified_by_fkey",
                table: "taxes_fees");

            migrationBuilder.DropForeignKey(
                name: "unit_created_by_fkey",
                table: "unit");

            migrationBuilder.DropForeignKey(
                name: "unit_modified_by_fkey",
                table: "unit");

            migrationBuilder.DropForeignKey(
                name: "orders_customer_id_fkey",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "invoice_section_id_fkey",
                table: "invoice");

            migrationBuilder.DropForeignKey(
                name: "tables_section_id_fkey",
                table: "tables");

            migrationBuilder.DropForeignKey(
                name: "invoice_table_id_fkey",
                table: "invoice");

            migrationBuilder.DropForeignKey(
                name: "invoice_order_id_fkey",
                table: "invoice");

            migrationBuilder.DropForeignKey(
                name: "order_tax_order_id_fkey",
                table: "order_tax");

            migrationBuilder.DropForeignKey(
                name: "ordersitemdetails_order_id_fkey",
                table: "ordersitemdetails");

            migrationBuilder.DropForeignKey(
                name: "ratings_order_id_fkey",
                table: "ratings");

            migrationBuilder.DropTable(
                name: "assign_table");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "customer_history");

            migrationBuilder.DropTable(
                name: "itemmodifier");

            migrationBuilder.DropTable(
                name: "roleswisepermissions");

            migrationBuilder.DropTable(
                name: "tableorder");

            migrationBuilder.DropTable(
                name: "waiting_list");

            migrationBuilder.DropTable(
                name: "state");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "tables");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "invoice");

            migrationBuilder.DropTable(
                name: "order_tax");

            migrationBuilder.DropTable(
                name: "modified_order_detail");

            migrationBuilder.DropTable(
                name: "taxes_fees");

            migrationBuilder.DropTable(
                name: "menu_modifier");

            migrationBuilder.DropTable(
                name: "ordersitemdetails");

            migrationBuilder.DropTable(
                name: "modifiers_group");

            migrationBuilder.DropTable(
                name: "menu_items");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "unit");
        }
    }
}
