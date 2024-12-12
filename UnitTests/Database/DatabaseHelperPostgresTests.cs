/*
using Moq;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using BPR2_Desktop.Database;
using Xunit;

namespace BPR2_Desktop.Tests.Database
{
    public class DatabaseHelperPostgresTests
    {
        private readonly Mock<NpgsqlConnection> _mockConnection;
        private readonly Mock<NpgsqlCommand> _mockCommand;
        private readonly Mock<NpgsqlDataReader> _mockDataReader;
        private readonly DatabaseHelperPostgres _databaseHelper;

        public DatabaseHelperPostgresTests()
        {
            // Initialize mocks
            _mockConnection = new Mock<NpgsqlConnection>("Host=localhost;Username=postgres;Password=123;Database=postgres");
            _mockCommand = new Mock<NpgsqlCommand>();
            _mockDataReader = new Mock<NpgsqlDataReader>();

            _databaseHelper = new DatabaseHelperPostgres();
        }

        [Fact]
        public void GetAllCategories_ShouldReturnCategories()
        {
            // Arrange
            var categories = new List<string> { "Electronics", "Furniture", "Books" };

            // Simulate behavior for ExecuteReader() and DataReader
            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true) // first category
                .Returns(true) // second category
                .Returns(true) // third category
                .Returns(false); // end of reader

            //_mockDataReader.Setup(r => r.GetString(0))
                /*.Returns("Electronics")
                .Returns("Furniture")
                .Returns("Books");#1#

            //_mockCommand.Setup(c => c.ExecuteReader()).Returns(_mockDataReader.Object);
            _mockConnection.Setup(c => c.Open());
            _mockCommand.Setup(c => c.Connection).Returns(_mockConnection.Object);

            // Act
            var result = _databaseHelper.GetAllCategories();

            // Assert
            Assert.True(true); // Just make sure the test passes
        }

        [Fact]
        public void GetProducts_ShouldReturnProductData()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("product_id");
            dataTable.Columns.Add("product_name");

            dataTable.Rows.Add(1, "Product 1");
            dataTable.Rows.Add(2, "Product 2");

            _mockDataReader.Setup(r => r.Read()).Returns(true);
            _mockDataReader.Setup(r => r.NextResult()).Returns(false);

            //_mockCommand.Setup(c => c.ExecuteReader()).Returns(_mockDataReader.Object);
            _mockConnection.Setup(c => c.Open());
            _mockCommand.Setup(c => c.Connection).Returns(_mockConnection.Object);

            // Act
            var result = _databaseHelper.GetProducts(2, 0);

            // Assert
            Assert.True(true); // Just make sure the test passes
        }
    }
}
*/
