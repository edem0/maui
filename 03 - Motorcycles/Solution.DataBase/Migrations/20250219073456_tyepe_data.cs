using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.Database.Migrations
{
    /// <inheritdoc />
    public partial class tyepe_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var query = @$"
                insert into
                    [Type]
                    ([Name])
                values
                    ('Chopper'),
                    ('Cruiser'),
                    ('Classic'),
                    ('Veteran'),
                    ('Cross'),
                    ('Pitpike'),
                    ('Enduro'),
                    ('Kids Bike'),
                    ('Sport'),
                    ('Quad'),
                    ('ATV'),
                    ('RUV'),
                    ('SSV'),
                    ('UTV'),
                    ('Scooter'),
                    ('Moped'),
                    ('Supermoto'),
                    ('Trial'),
                    ('Trike'),
                    ('Tour'),
                    ('Naked'),
                    ('Sport Tour')
            ";

            migrationBuilder.Sql(query);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var query = $"truncate table [Type]";

            migrationBuilder.Sql(query);
        }
    }
}
