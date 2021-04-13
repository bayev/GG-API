﻿using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : Controller
    {
        private Context _context;
        private readonly UserManager<User> _userManager;
        public ProductsController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllProducts()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                List<Product> listOfProducts = new List<Product>();

                if (!listOfProducts.Any())
                {
                    var query = _context.Products;

                    foreach (var item in query)
                    {
                        Product product = new Product();
                        product.Id = item.Id;
                        product.Name = item.Name;
                        product.Price = item.Price;
                        product.Weight = item.Weight;
                        product.Description = item.Description;
                        product.Image = item.Image;
                        product.Category = item.Category;
                        product.CreateDate = item.CreateDate;
                        product.Stock = item.Stock;
                        product.Size = item.Size;
                        product.Brand = item.Brand;
                        product.Discount = item.Discount;

                        listOfProducts.Add(product);
                    }
                    return Ok(listOfProducts);

                }
                else
                    return NotFound(new { message = "Sorry, no products found" });
            }
            else
                return Unauthorized();
        }

        [HttpPut("update/{productid}")]
        public async Task<ActionResult> UpdateProduct([FromBody] PostProductModel model, string Productid)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    Product product = _context.Products.Where(x => x.Id == Productid).FirstOrDefault();
                    product.Name = model.Name;
                    product.Price = Convert.ToDecimal(model.Price);
                    product.Weight = model.Weight;
                    product.Description = model.Description;
                    // upload image the proper way
                    product.Image = model.Image;
                    product.Category = model.Category;
                    product.Stock = model.Stock;
                    product.Size = model.Size;
                    product.Brand = model.Brand;
                    product.Discount = model.Discount;
                    product.Highlighted = model.Highlighted;

                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"Sorry, something happend. {ex.ToString()}" });
                }
            }
            else
                return Unauthorized();
        }

        [HttpGet("get/{productid}")]
        public async Task<ActionResult> GetProduct([FromRoute] string productid)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                Product product = _context.Products.Where(x => x.Id == productid).FirstOrDefault();

                return Ok(product);
            }
            else
                return Unauthorized();
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct([FromBody] PostProductModel model)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                Product product = new Product();

                product.Name = model.Name;
                product.Price = Convert.ToDecimal(model.Price);
                product.Weight = model.Weight;
                product.Description = model.Description;

                // upload image the proper way
                product.Image = model.Image;
                product.Category = model.Category;
                product.Stock = model.Stock;
                product.Size = model.Size;
                product.Brand = model.Brand;
                product.Highlighted = model.Highlighted;

                try
                {
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"Sorry, something happend. {ex.ToString()}" });
                }

                return Ok();

            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpDelete("delete/{productid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] string productid)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    Product product = _context.Products.Where(x => x.Id == productid).FirstOrDefault();
                    _context.Remove(product);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"Sorry, something happened. {ex.ToString()}" });

                }
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        [AllowAnonymous]
        [HttpGet("GGProducts")] //EXPOSED METHOD FOR OUR PRODUCTS TO BE DISPLAYED
        public async Task<ActionResult> GGProducts()
        {

            try
            {
                var products = _context.Products;
                var exposedList = new List<ExposedProducts>();

                foreach (var item in products)
                {
                    var tempInstance = new ExposedProducts
                    {
                        Name = item.Name,
                        Price = item.Price,
                        Description = item.Description,
                        Image = item.Image
                    };
                    exposedList.Add(tempInstance);
                }

                return Ok(exposedList);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Could not get our products at this moment. {ex.ToString()}" });

            }


        }
        [HttpPost("seed")]
        public async void SeedDB() //ANVÄND POSTMAN FÖR ATT SEEDA DB. Method POST, använd TOKEN /products/seed
        {
            List<Product> seedList = new List<Product>();

            Product product10 = new Product();
            product10.Name = "Morris Pullover";
            product10.Price = 599;
            product10.Weight = 400;
            product10.Description = "En stilren pullover från Morris som säkrar vardagsstilen en kall och mulen höstdag eller en sensommarkväll.";
            product10.Image = "MorrisSweater.jpg";
            product10.Category = "Sweaters";
            product10.CreateDate = DateTime.Now;
            product10.Stock = 10;
            product10.Size = "M";
            product10.Brand = "Morris";

            Product product1 = new Product();
            product1.Name = "Calvin Klein tröja";
            product1.Price = 799;
            product1.Weight = 425;
            product1.Description = "Grå sweater från CK, en stilren tröja som funkar både i hemmet och på stan.";
            product1.Image = "CkSweater.jpg";
            product1.Category = "Sweaters";
            product1.CreateDate = DateTime.Now;
            product1.Stock = 10;
            product1.Size = "L";
            product1.Brand = "Calvin Klein";

            Product product2 = new Product();
            product2.Name = "Ralph Lauren skjorta";
            product2.Price = 699;
            product2.Weight = 355;
            product2.Description = "Stilren, svart skjorta från RL. Bär till jeans eller kostym, inga ursäkter för att inte bära denna sköna skjorta.";
            product2.Image = "RalphLaurenShirt.jpg";
            product2.Category = "Shirts";
            product2.CreateDate = DateTime.Now;
            product2.Stock = 10;
            product2.Size = "S";
            product2.Brand = "Ralph Lauren";

            Product product3 = new Product();
            product3.Name = "Philippa K kostymbyxor";
            product3.Price = 1499;
            product3.Weight = 445;
            product3.Description = "Ett par stilrena kostymbyxor från Philippa K är du säker på att du håller stilen, passar till vardags så väl som till fest.";
            product3.Image = "PhilippaKTrousers.jpg";
            product3.Category = "Pants";
            product3.CreateDate = DateTime.Now;
            product3.Stock = 10;
            product3.Size = "M";
            product3.Brand = "Philippa K";

            Product product5 = new Product();
            product5.Name = "Rains ryggsäck";
            product5.Price = 899;
            product5.Weight = 500;
            product5.Description = "Men en ryggsäck från Rains får du med dig laptop, ipad, och andra atteraljer utan att tumma på stilen. Gjord i vattentätt material så din elektronik inte blir förstörd om det skulle regna.";
            product5.Image = "RainsBackpack.jpg";
            product5.Category = "Diverse";
            product5.CreateDate = DateTime.Now;
            product5.Stock = 10;
            product5.Size = null;
            product5.Brand = "Rain";

            Product product6 = new Product();
            product6.Name = "Tiger of Sweden Blazer";
            product6.Price = 2499;
            product6.Weight = 500;
            product6.Description = "Med en blazer från Tiger of Sweden kan du vara säker på att du håll stilen både till fest och vardag. Gjord i kvalitetsmaterial med bekväm passform.";
            product6.Image = "TigerBlazer.jpg";
            product6.Category = "Blazers";
            product6.CreateDate = DateTime.Now;
            product6.Stock = 5;
            product6.Size = "M";
            product6.Brand = "Tiger of Sweden";

            Product product7 = new Product();
            product7.Name = "Dolce & Gabbana Chinos";
            product7.Price = 1199;
            product7.Weight = 350;
            product7.Description = "Säkra stilen med grå-rutiga chinos från [brand] med skön passform och kvalitetsmaterial.";
            product7.Image = "DolceNgabbanaChinos.jpg";
            product7.Category = "Pants";
            product7.CreateDate = DateTime.Now;
            product7.Stock = 5;
            product7.Size = "L";
            product7.Brand = "Dolce & Gabbana";

            Product product8 = new Product();
            product8.Name = "Plånbok från Hugo Boss";
            product8.Price = 699;
            product8.Weight = 50;
            product8.Description = "Med en plånbok från Hugo Boss kommer du inte dra dig från att vara en gentleman och betala notan. Allt för att visa upp denna stilrena, praktiska plånbok med plats för alla dina kort och kontanter.";
            product8.Image = "HugoBossWallet.jpg";
            product8.Category = "Diverse";
            product8.CreateDate = DateTime.Now;
            product8.Stock = 10;
            product8.Size = null;
            product8.Brand = "Hugo Boss";

            Product product9 = new Product();
            product9.Name = "Manschettknappar från Skultuna";
            product9.Price = 499;
            product9.Weight = 50;
            product9.Description = "Snygga till din vardag med manschettknappar från Skultuna. En snygg, stilren detalj som passar till de flesta skortor och tillfällen. Tillverkade i 18k guldpläterad mässing prydd med Tre kronor.";
            product9.Image = "Manschett.jpg";
            product9.Category = "Accessories";
            product9.CreateDate = DateTime.Now;
            product9.Stock = 10;
            product9.Size = null;
            product9.Brand = "Skultuna";

            Product az1 = new Product();
            az1.Name = "Blazor från Tommy Hilfiger";
            az1.Price = 2229;
            az1.Weight = 700;
            az1.Description = "Sharpen up your workwear wardrobe with this tailored double breasted blazer, finished with sleek polished buttons.";
            az1.Image = "TommyGilfinger.jpg";
            az1.Category = "Blazor";
            az1.CreateDate = DateTime.Now;
            az1.Stock = 10;
            az1.Size = null;
            az1.Brand = "Tommy Hilfiger";

            Product az2 = new Product();
            az2.Name = "Blazor från Filippa K";
            az2.Price = 3699;
            az2.Weight = 800;
            az2.Description = "En vardaglig kavaj i stor passform, inspirerad av det marina i sin blå färg med guldfärgade knappar och diskret dragskomidja för en smickrande siluett.";
            az2.Image = "FilippaK.jpg";
            az2.Category = "Blazor";
            az2.CreateDate = DateTime.Now;
            az2.Stock = 7;
            az2.Size = null;
            az2.Brand = "Filippa K";

            Product az3 = new Product();
            az3.Name = "Blazor från Oscar Jacobson";
            az3.Price = 4499;
            az3.Weight = 650;
            az3.Description = "En vardaglig kavaj i stor passform, i sin gröna färg med svarta knappar.";
            az3.Image = "OscarJacobson.jpg";
            az3.Category = "Blazor";
            az3.CreateDate = DateTime.Now;
            az3.Stock = 13;
            az3.Size = null;
            az3.Brand = "Oscar Jacobson";

            Product az4 = new Product();
            az4.Name = "Blazor från Polo Ralph Lauren";
            az4.Price = 5999;
            az4.Weight = 750;
            az4.Description = "Snygg dubbelknäppt kavaj med guldfärgade knappar från Polo Ralph Lauren Womenswear. Feminin passform med två fickor framtill.";
            az4.Image = "PoloRalp.jpg";
            az4.Category = "Blazor";
            az4.CreateDate = DateTime.Now;
            az4.Stock = 12;
            az4.Size = null;
            az4.Brand = "Ralph Lauren";

            Product tess1 = new Product();
            tess1.Name = "Gant Klocka";
            tess1.Price = 499;
            tess1.Weight = 150;
            tess1.Description = "Denna stilrena klockan från Gant gör din outfit komplett. Med blå urtavla är den klassiskt tidlös.";
            tess1.Image = "GantKlocka.jpg";
            tess1.Category = "Accessories";
            tess1.CreateDate = DateTime.Now;
            tess1.Stock = 10;
            tess1.Size = null;
            tess1.Brand = "Gant";

            Product tess2 = new Product();
            tess2.Name = "Slipsnål";
            tess2.Price = 349;
            tess2.Weight = 80;
            tess2.Description = "Stilren slipsnål i äkta silver, med svarta detaljer.";
            tess2.Image = "Slipsnål.jpg";
            tess2.Category = "Accessories";
            tess2.CreateDate = DateTime.Now;
            tess2.Stock = 10;
            tess2.Size = null;
            tess2.Brand = "";

            Product tess3 = new Product();
            tess3.Name = "Läderarmband";
            tess3.Price = 799;
            tess3.Weight = 60;
            tess3.Description = "Stilrent läderarmband med silver-detaljer blir en snygg detalj för den som inte vill bära klocka.";
            tess3.Image = "ArmbandLäder.jpg";
            tess3.Category = "Accessories";
            tess3.CreateDate = DateTime.Now;
            tess3.Stock = 10;
            tess3.Size = null;
            tess3.Brand = "Hugo Boss";

            Product tess4 = new Product();
            tess4.Name = "RayBan Aviator";
            tess4.Price = 899;
            tess4.Weight = 80;
            tess4.Description = "Klassiska solglasögon från RayBan i Aviator-utförande, ett måste till soliga sommardagar.";
            tess4.Image = "RayBanAviator.jpg";
            tess4.Category = "Accessories";
            tess4.CreateDate = DateTime.Now;
            tess4.Stock = 10;
            tess4.Size = null;
            tess4.Brand = "RayBan";

            seedList.Add(product10);
            seedList.Add(product1);
            seedList.Add(product2);
            seedList.Add(product3);
            seedList.Add(product5);
            seedList.Add(product6);
            seedList.Add(product7);
            seedList.Add(product8);
            seedList.Add(product9);
            seedList.Add(az1);
            seedList.Add(az2);
            seedList.Add(az3);
            seedList.Add(az4);

            foreach (var item in seedList)
            {
                _context.Add(item);
            }

            _context.SaveChanges();
        }

    }
}
