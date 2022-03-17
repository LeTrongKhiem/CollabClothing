using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CollabClothing.WebApp.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameAction = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannerType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameBannerType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    NameBrand = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Info = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Images = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameCategory = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ParentId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Icon = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsShowWeb = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameColor = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameFunction = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ParentId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameRole = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameSize = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameTopic = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime", nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NameBanner = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Images = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Alt = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TypeBannerId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Text = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                    table.ForeignKey(
                        name: "fk_bannertype_banner",
                        column: x => x.TypeBannerId,
                        principalTable: "BannerType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PriceCurrent = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    PriceOld = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    SaleOff = table.Column<int>(type: "int", nullable: true),
                    BrandId = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    SoldOut = table.Column<bool>(type: "bit", nullable: false),
                    Installment = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "fk_brand_product",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    FunctionId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ActionId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_action_per",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_func_per",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_role_per",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SizeMapColor",
                columns: table => new
                {
                    Sizeid = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ColorId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_color_size",
                        column: x => x.ColorId,
                        principalTable: "Color",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_size_color",
                        column: x => x.Sizeid,
                        principalTable: "Size",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TopicId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UserPhone = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UserEmail = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "fk_topic_contact",
                        column: x => x.TopicId,
                        principalTable: "Topic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ShipName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ShipAddress = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ShipEmail = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ShipPhoneNumber = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "fk_user_order",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemActivity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ActionName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FunctionId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClientIP = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemActivity", x => x.Id);
                    table.ForeignKey(
                        name: "fk_func_sys",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_sys",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_role_user",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_role",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_cart",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_productDetails",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Alt = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_productImage",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductMapCategory",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CategoryId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductM__159C556D63F46B0A", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "fk_cat_productmapcat",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_product_productMapCat",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductMapSize",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    SizeId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductM__0C37165AA0FEF2ED", x => new { x.ProductId, x.SizeId });
                    table.ForeignKey(
                        name: "fk_product_size",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_size_product",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NamePromotion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                    table.ForeignKey(
                        name: "fk_product_promotion",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    OrderId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "fk_order_orderdetails",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromotionDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PromotionId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    OnlinePromotion = table.Column<bool>(type: "bit", nullable: false),
                    Info = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    More = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "fk_promo_promodetail",
                        column: x => x.PromotionId,
                        principalTable: "Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "Id", "Images", "Info", "NameBrand", "Slug" },
                values: new object[,]
                {
                    { "01", "adidas.png", "Brand Adidas", "Adidas", "/adidas" },
                    { "02", "nike.png", "Brand Nike", "Nike", "/nike" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Icon", "IsShowWeb", "Level", "NameCategory", "ParentId", "Slug" },
                values: new object[,]
                {
                    { "10001", "men", true, 1, "Men", "null", "/men" },
                    { "10002", "women", true, 1, "Women", "null", "/women" },
                    { "20001", "men", true, 2, "T-Shirt", "10001", "/men/t-shirt" },
                    { "20002", "women", true, 2, "T-Shirt", "10002", "/women/t-shirt" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "BrandId", "Description", "Installment", "PriceCurrent", "PriceOld", "ProductName", "SaleOff", "Slug", "SoldOut" },
                values: new object[,]
                {
                    { "01", "01", "", 0, 300000m, 450000m, "Ao thun nam ", 20, "/", false },
                    { "03", "01", "", 0, 200000m, 350000m, "Ao thun nu", 20, "/", false },
                    { "02", "02", "", 0, 400000m, 450000m, "Ao thun nam 2", 20, "/", true },
                    { "04", "02", "", 0, 200000m, 320000m, "Ao thun nu 1", 20, "/", false }
                });

            migrationBuilder.InsertData(
                table: "ProductMapCategory",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { "20001", "01" });

            migrationBuilder.InsertData(
                table: "ProductMapCategory",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { "20002", "02" });

            migrationBuilder.CreateIndex(
                name: "IX_Banner_TypeBannerId",
                table: "Banner",
                column: "TypeBannerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductId",
                table: "Cart",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_TopicId",
                table: "Contact",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_ActionId",
                table: "Permission",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_FunctionId",
                table: "Permission",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_RoleId",
                table: "Permission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandId",
                table: "Product",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMapCategory_CategoryId",
                table: "ProductMapCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMapSize_SizeId",
                table: "ProductMapSize",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_ProductId",
                table: "Promotion",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionDetail_PromotionId",
                table: "PromotionDetail",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_SizeMapColor_ColorId",
                table: "SizeMapColor",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_SizeMapColor_Sizeid",
                table: "SizeMapColor",
                column: "Sizeid");

            migrationBuilder.CreateIndex(
                name: "IX_SystemActivity_FunctionId",
                table: "SystemActivity",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemActivity_UserId",
                table: "SystemActivity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "ProductDetail");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "ProductMapCategory");

            migrationBuilder.DropTable(
                name: "ProductMapSize");

            migrationBuilder.DropTable(
                name: "PromotionDetail");

            migrationBuilder.DropTable(
                name: "SizeMapColor");

            migrationBuilder.DropTable(
                name: "SystemActivity");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "BannerType");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Brand");
        }
    }
}
