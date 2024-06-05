using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;




namespace BooksApi.Controllers
{

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
    {
       
      private readonly string _filePath = "";
      
        public BooksController(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data","Books.xml");
        }

        // GET: api/Books
        [HttpGet]
    public IEnumerable<Book> Get()
    {
        return ParseBooksFromFile();
    }

        // GET: api/Books/{isbn}
        [HttpGet("{isbn}")]
        public Book Get(string isbn)
    {
        var books = ParseBooksFromFile();
        return books.FirstOrDefault(b => b.Isbn == isbn);
    }

        // POST: api/Books
        [HttpPost]
        public void Post(Book book)
    {
        var books = ParseBooksFromFile();
        books.Add(book);
        SaveBooksToFile(books);
    }

        // PUT: api/Books/{isbn}
        [HttpPut("{isbn}")]
        public void Put(string isbn, Book book)
    {
        var books = ParseBooksFromFile();
        var index = books.FindIndex(b => b.Isbn == isbn);
        if (index > -1)
        {
            books[index] = book;
            SaveBooksToFile(books);
        }
    }

        // DELETE: api/Books/{isbn}
        [HttpDelete("{isbn}")]
        public void Delete(string isbn)
    {
        var books = ParseBooksFromFile();
        books.RemoveAll(b => b.Isbn == isbn);
        SaveBooksToFile(books);
    }

        private List<Book> ParseBooksFromFile()
        {
            var books = new List<Book>();
            if (System.IO.File.Exists(_filePath))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(_filePath);

                var bookstoreNode = xmlDoc.DocumentElement; // Assuming root element is "bookstore"

                foreach (XmlNode bookNode in bookstoreNode.ChildNodes)
                {
                    if (bookNode.Name == "book")
                    {
                        var book = new Book
                        {
                            Isbn = bookNode["isbn"]?.InnerText ??"",
                            Title = bookNode["title"]?.InnerText??"",
                            Language = bookNode["title"]?.Attributes["lang"]?.Value ?? "", // Check for language attribute
                            Authors = new List<string>(),
                            Category = bookNode.Attributes["category"]?.Value ?? "", // Check for category attribute
                            Cover = bookNode.Attributes["cover"]?.Value ?? "" // Check for cover attribute
                        };

                        foreach (XmlNode authorNode in bookNode.SelectNodes("author"))
                        {
                            book.Authors.Add(authorNode.InnerText);
                        }

                        var yearNode = bookNode.SelectSingleNode("year");
                        book.Year = int.Parse(yearNode.InnerText);

                        var priceNode = bookNode.SelectSingleNode("price");
                        book.Price = decimal.Parse(priceNode.InnerText);

                        books.Add(book);
                    }
                }
            }
            return books;
        }

        private void SaveBooksToFile(List<Book> books)
    {
        var xmlDoc = new XmlDocument();
        var rootNode = xmlDoc.CreateElement("bookstore");
        xmlDoc.AppendChild(rootNode);

        foreach (var book in books)
        {
            var bookNode = xmlDoc.CreateElement("book");
            rootNode.AppendChild(bookNode);


            var isbnNode = xmlDoc.CreateElement("isbn");
            isbnNode.InnerText = book.Isbn;
            bookNode.AppendChild(isbnNode);

            var titleNode = xmlDoc.CreateElement("title");
            titleNode.InnerText = book.Title;
            titleNode.SetAttribute("lang", book.Language);
            bookNode.AppendChild(titleNode);

            foreach (var author in book.Authors)
            {
                var authorNode = xmlDoc.CreateElement("author");
                authorNode.InnerText = author;
                bookNode.AppendChild(authorNode);
            }

            var yearNode = xmlDoc.CreateElement("year");
            yearNode.InnerText = book.Year.ToString();
            bookNode.AppendChild(yearNode);

            var priceNode = xmlDoc.CreateElement("price");
            priceNode.InnerText = book.Price.ToString();
            bookNode.AppendChild(priceNode);
        }

        xmlDoc.Save(_filePath);
    }
}
}




