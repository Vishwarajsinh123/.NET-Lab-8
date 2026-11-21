using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using inventory_management_module.Models;

namespace inventory_management_module.Controllers
{
    public class ProductController : Controller
    {
        string cs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InventoryDB;Integrated Security=True";

        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "Select * from Products";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"])
                    });
                }
            }
            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        // CREATE: Save Product
        [HttpPost]
        public ActionResult Create(Product product)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Products
                         (ProductName, Quantity, Price)
                         VALUES (@ProductName, @Quantity, @Price)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Price", product.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // UPDATE: Display existing products
        public ActionResult Edit(int id)
        {
            Product product = new Product();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "Select * from Products WHERE " +
                               "ProductID=@ProductID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductID", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product.ProductID = Convert.ToInt32(reader["ProductID"]);
                    product.ProductName = reader["ProductName"].ToString();
                    product.Quantity = Convert.ToInt32(reader["Quantity"]);
                    product.Price = Convert.ToDecimal(reader["Price"]);
                } else
                {
                    return HttpNotFound();
                }
            }
            return View(product);
        }

        // UPDATE: Save Changes
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Products SET
                         ProductName = @ProductName,
                         Quantity = @Quantity,
                         Price = @Price
                         WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Price", product.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // DELETE: Remove Product
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Products WHERE ProductID=@ProductID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}