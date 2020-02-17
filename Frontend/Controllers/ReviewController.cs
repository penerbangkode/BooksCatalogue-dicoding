using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BooksCatalogue.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;


namespace BooksCatalogue.Controllers
{
    public class ReviewController : Controller
    {
        private string apiEndpoint = "https://ruiux.azurewebsites.net//api/";
        public HttpClient _client;
        HttpClientHandler clientHandler = new HttpClientHandler();

        public string baseUrl = "https://frontlien.azurewebsites.net//Books/Details/";
        public ReviewController() {
            
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Use this client handler to bypass ssl policy errors
            // clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
           _client = new HttpClient(clientHandler);
         
            
            
        }
        
        // GET: Review/AddReview/2
        public async Task<IActionResult> AddReview(int? bookId)
        {
            
         
             
            if (bookId == null)
            {
                return NotFound();
            }

             _client = new HttpClient(clientHandler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint + "books/" + bookId);

            HttpResponseMessage response = await _client.SendAsync(request);

            switch(response.StatusCode)
            {
                case HttpStatusCode.OK:
                    string responseString = await response.Content.ReadAsStringAsync();
                    var book = JsonSerializer.Deserialize<Book>(responseString);

                    ViewData["BookId"] = bookId;
                    return View("AddReview");
                case HttpStatusCode.NotFound:
                    return NotFound();
                default:
                    return ErrorAction("Error. Status code = " + response.StatusCode + ": " + response.ReasonPhrase);
            }
        }



        // TODO: Tambahkan fungsi ini untuk mengirimkan atau POST data review menuju API
        // POST: Review/AddReview
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> AddReview([Bind("Id,BookId,ReviewerName,Rating,Comment")] [FromForm] Review review)
        {

            _client = new HttpClient(clientHandler);
            MultipartFormDataContent content = new MultipartFormDataContent();


            content.Add(new StringContent(review.BookId.ToString()), "bookId");
            content.Add(new StringContent(review.ReviewerName),"reviewername");
            content.Add(new StringContent(review.Rating.ToString()),"rating");
            content.Add(new StringContent(review.Comment), "comment");
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint + "review/");
            request.Content = content;
            HttpResponseMessage response = await _client.SendAsync(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        int bookids = review.BookId;
                        return Redirect(baseUrl + bookids);
                    default:
                        return ErrorAction("Error. Status code = " + response.StatusCode + "; " + response.ReasonPhrase);
                }
            
        }
        public async Task<IActionResult> Delete(int? id)
        {
             
            if (id == null)
            {
                return NotFound();
            }
             _client = new HttpClient(clientHandler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint + "review/" + id);

            HttpResponseMessage response = await _client.SendAsync(request);

            switch(response.StatusCode)
            {
                case HttpStatusCode.OK:
                    string responseString = await response.Content.ReadAsStringAsync();
                    var book = JsonSerializer.Deserialize<Review>(responseString);

                    
                    return View(book);
                default:
                    return ErrorAction("Error. Status code = " + response.StatusCode + ": " + response.ReasonPhrase);
            }
        }

        [HttpPost, ActionName("DeleteReview")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            _client = new HttpClient(clientHandler);
        
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, apiEndpoint + "review/" + id);
            
            HttpResponseMessage response = await _client.SendAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    return Redirect("https://frontlien.azurewebsites.net/");
                case HttpStatusCode.Unauthorized:
                    return ErrorAction("Please sign in again. " + response.ReasonPhrase);
                default:
                    return ErrorAction("Error. Status code = " + response.StatusCode );
            }
        }
    
        private ActionResult ErrorAction(string message)
        {
            
            return new RedirectResult("/Home/Error?message=" + message);
        }
    }
}