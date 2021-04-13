using Api.Data;
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

            Product product11 = new Product();
            product11.Name = "Grant Cotton/Linen Trousers Navy";
            product11.Price = 1299;
            product11.Weight = 200;
            product11.Description = "Byxa med side-adjusters och pressveck i en avsmalnande passform från svenska J.Lindeberg. Byxorna är tillverkade i en stretchig lin- och bomullskvalitet, perfekt för årets varmare dagar..";
            product11.Image = "JLinderbergByxor.jpg";
            product11.Category = "Pants";
            product11.CreateDate = DateTime.Now;
            product11.Stock = 10;
            product11.Size = "32";
            product11.Brand = "J.Linderberg";


            Product product12 = new Product();
            product12.Name = "Jogger Sweatpants Light Sport Heather";
            product12.Price = 1199;
            product12.Weight = 250;
            product12.Description = "Mjukisbyxa med avsmalnande passform från Polo Ralph Laurens performance-kollektion. Tillverkad i en slät dubbelstickad bomullsblandning för ett mer modernt och elegant utseende..";
            product12.Image = "RLbyxor.jpg";
            product12.Category = "Pants";
            product12.CreateDate = DateTime.Now;
            product12.Stock = 10;
            product12.Size = "L";
            product12.Brand = "Polo Ralph Lauren";

            Product product13 = new Product();
            product13.Name = "Danwick Side Adjusters Chino Beige";
            product13.Price = 1199;
            product13.Weight = 210;
            product13.Description = "Byxor med avsmalnande passform från Oscar Jacobson. Tillverkade i bomull med stretch och side-adjusters vilket ger byxan en både uppklädd och avslappnad känsla.";
            product13.Image = "OJbyxor.jpg";
            product13.Category = "Pants";
            product13.CreateDate = DateTime.Now;
            product13.Stock = 10;
            product13.Size = "50";
            product13.Brand = "Oscar Jacobson";

            Product product14 = new Product();
            product14.Name = "Howard Pinstripe Drawstring Pants Dark Blue";
            product14.Price = 1599;
            product14.Weight = 200;
            product14.Description = "Kritstrecksrandig drawstring-byxa från HUGO. Tillverkade i en stretchig bomullsblandning med pressveck som ger en bra balans mellan dressat och ledigt.";
            product14.Image = "HBbyxor.jpg";
            product14.Category = "Pants";
            product14.CreateDate = DateTime.Now;
            product14.Stock = 10;
            product14.Size = "42";
            product14.Brand = "HUGO BOSS";

            Product product15 = new Product();
            product15.Name = "502™ TAPER HI-BALL JEANS";
            product15.Price = 699;
            product15.Weight = 700;
            product15.Description = "De ultimata jeansen från LEVIS för sneakers. Smala med överdriven avsmalning. Kommer upprullade och redo att visa upp dina skor. Tillverkade med +Levi's® Flex: vår mest avancerade stretchteknik, konstruerad för att ge största möjliga flex och optimal komfort.";
            product15.Image = "LevisJeans.jpg";
            product15.Category = "Pants";
            product15.CreateDate = DateTime.Now;
            product15.Stock = 10;
            product15.Size = "34";
            product15.Brand = "LEVIS 502™";

            Product product16 = new Product();
            product16.Name = "Slim Fit Chambray Shirt Washed";
            product16.Price = 1299;
            product16.Weight = 170;
            product16.Description = "Chambrayskjorta från Polo Ralph Lauren. Varumärket har blivit en symbol för autenticitet genom en kombination av klassisk engelsk stil och den amerikanska östkustens avslappnade estetik.";
            product16.Image = "RLshirtBlue.jpg";
            product16.Category = "Shirts";
            product16.CreateDate = DateTime.Now;
            product16.Stock = 10;
            product16.Size = "L";
            product16.Brand = "Polo Ralph Lauren";

            Product product17 = new Product();
            product17.Name = "Custom Fit Striped Linen Shirt Olive/White";
            product17.Price = 1499;
            product17.Weight = 150;
            product17.Description = "Linneskjorta från Polo Ralph Lauren. Varumärket har blivit en symbol för autenticitet genom en kombination av klassisk engelsk stil och den amerikanska östkustens avslappnade estetik.";
            product17.Image = "RLlightShirt.jpg";
            product17.Category = "Shirts";
            product17.CreateDate = DateTime.Now;
            product17.Stock = 10;
            product17.Size = "M";
            product17.Brand = "Polo Ralph Lauren";

            Product product18 = new Product();
            product18.Name = "Slimline Striped Linen Cut Away Shirt Olive";
            product18.Price = 1599;
            product18.Weight = 180;
            product18.Description = "Linneskjorta från Stenströms. Tillverkad i en tunn och sval linneväv med knappar i pärlemor, idealisk att bära varma vår- och sommardagar till chinos eller shorts.";
            product18.Image = "StenShirt.jpg";
            product18.Category = "Shirts";
            product18.CreateDate = DateTime.Now;
            product18.Stock = 10;
            product18.Size = "XL";
            product18.Brand = "Stenströms";

            Product product19 = new Product();
            product19.Name = "Slim Fit Oxford Shirt Brimstone Yellow";
            product19.Price = 999;
            product19.Weight = 180;
            product19.Description = "Klassisk oxfordskjorta från GANT. Varumärket är starkt förknippat med amerikansk preppystil och klassiska plagg med influenser hämtade från IVY-League universitet Yale, New Haven.";
            product19.Image = "RLyellowShirt.jpg";
            product19.Category = "Shirts";
            product19.CreateDate = DateTime.Now;
            product19.Stock = 10;
            product19.Size = "L";
            product19.Brand = "GANT";


            Product az1 = new Product();
            az1.Name = "Blazer från Tommy Hilfiger";
            az1.Price = 2229;
            az1.Weight = 700;
            az1.Description = "Sharpen up your workwear wardrobe with this tailored double breasted blazer, finished with sleek polished buttons.";
            az1.Image = "TommyGilfinger.jpg";
            az1.Category = "Blazers";
            az1.CreateDate = DateTime.Now;
            az1.Stock = 10;
            az1.Size = null;
            az1.Brand = "Tommy Hilfiger";

            Product az2 = new Product();
            az2.Name = "Blazer från Filippa K";
            az2.Price = 3699;
            az2.Weight = 800;
            az2.Description = "En vardaglig kavaj i stor passform, inspirerad av det marina i sin blå färg med guldfärgade knappar och diskret dragskomidja för en smickrande siluett.";
            az2.Image = "FilippaK.jpg";
            az2.Category = "Blazers";
            az2.CreateDate = DateTime.Now;
            az2.Stock = 7;
            az2.Size = null;
            az2.Brand = "Filippa K";

            Product az3 = new Product();
            az3.Name = "Blazer från Oscar Jacobson";
            az3.Price = 4499;
            az3.Weight = 650;
            az3.Description = "En vardaglig kavaj i stor passform, i sin gröna färg med svarta knappar.";
            az3.Image = "OscarJacobson.jpg";
            az3.Category = "Blazers";
            az3.CreateDate = DateTime.Now;
            az3.Stock = 13;
            az3.Size = null;
            az3.Brand = "Oscar Jacobson";

            Product az4 = new Product();
            az4.Name = "Blazer från Polo Ralph Lauren";
            az4.Price = 5999;
            az4.Weight = 750;
            az4.Description = "Snygg dubbelknäppt kavaj med guldfärgade knappar från Polo Ralph Lauren Womenswear. Feminin passform med två fickor framtill.";
            az4.Image = "PoloRalp.jpg";
            az4.Category = "Blazers";
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

            Product matte1 = new Product();
            matte1.Name = "Gant Hoodie Evening Blue";
            matte1.Price = 1000;
            matte1.Weight = 399;
            matte1.Description = "Tröja tillverkad i en mjuk bomullsblandning med broderad logotyp och magficka.";
            matte1.Image = "GantBlueSweater.jpg";
            matte1.Category = "Sweaters";
            matte1.CreateDate = DateTime.Now;
            matte1.Stock = 10;
            matte1.Size = "M";
            matte1.Brand = "Gant";

            Product matte2 = new Product();
            matte2.Name = "BOSS Athleisure Block Hoodie Black";
            matte2.Price = 1999;
            matte2.Weight = 399;
            matte2.Description = "Huvtröja från BOSS Athleisure. Tillverkad i en mjuk bomull- och syntetblandning med en sportig känsla, broderad logotyp på bröstet.";
            matte2.Image = "HugoBossBlackSweater.jpg";
            matte2.Category = "Sweaters";
            matte2.CreateDate = DateTime.Now;
            matte2.Stock = 10;
            matte2.Size = "S";
            matte2.Brand = "Hugo Boss";

            Product matte3 = new Product();
            matte3.Name = "Ralph Lauren Cotton Cable Pullover";
            matte3.Price = 1499;
            matte3.Weight = 259;
            matte3.Description = "Kabelstickad tröja med rund hals från Polo Ralph Lauren. Tillverkad i en mjuk bomullskvalitet och prydd med en broderad polospelare på vänster bröst.";
            matte3.Image = "RalphLaurenWhiteSweater.jpg";
            matte3.Category = "Sweaters";
            matte3.CreateDate = DateTime.Now;
            matte3.Stock = 10;
            matte3.Size = "L";
            matte3.Brand = "Ralph Lauren";

            Product matte4 = new Product();
            matte4.Name = "Morris Merino John Half-Zip Navy";
            matte4.Price = 1699;
            matte4.Weight = 415;
            matte4.Description = "Finstickad zip-tröja med dragkedja i kragen från Morris. Tillverkad i merinoull - ett ekologiskt framtaget ullgarn med en hög komfort och mjuk känsla, producerat av Biella Yarn.";
            matte4.Image = "MorrisNavySweater.jpg";
            matte4.Category = "Sweaters";
            matte4.CreateDate = DateTime.Now;
            matte4.Stock = 10;
            matte4.Size = "M";
            matte4.Brand = "Morris";

            Product matte5 = new Product();
            matte5.Name = "Calvin Klein Hoodie Shocking Orange";
            matte5.Price = 1099;
            matte5.Weight = 369;
            matte5.Description = "Huvtröja från Calvin Klein i en kraftig bomullskvalitet med tryckt logotyp på bröstet. En avslappnad tröja med mjukt ruggad insida som bjuder på en hög komfortnivå.";
            matte5.Image = "CkOrangeSweater.jpg";
            matte5.Category = "Sweaters";
            matte5.CreateDate = DateTime.Now;
            matte5.Stock = 10;
            matte5.Size = "XS";
            matte5.Brand = "Calvin Klein";

            Product matte6 = new Product();
            matte6.Name = "Peak Performance Full Zip Hoodie Black";
            matte6.Price = 1199;
            matte6.Weight = 379;
            matte6.Description = "Huvtröja med dragkedja från Peak Performance. Tillverkad i en mjuk och slitstark bomullskvalitet, med ruggad insida för ökad komfort, samt en broderad logotyp på vänster bröst.";
            matte6.Image = "PeakPerformanceBlackHoodie.jpg";
            matte6.Category = "Sweaters";
            matte6.CreateDate = DateTime.Now;
            matte6.Stock = 10;
            matte6.Size = "S";
            matte6.Brand = "Peak Performance";

            Product matte7 = new Product();
            matte7.Name = "Ralph Lauren Athletic Fleece Hoodie Red";
            matte7.Price = 1499;
            matte7.Weight = 470;
            matte7.Description = "Huvtröja från Polo Ralph Lauren. Tillverkad i en mjuk jersey av bomull och polyester med ruggad insida för ökad komfort. Den ikoniska polospelaren broderad på vänster bröst.";
            matte7.Image = "RalphLaurenRedSweater.jpg";
            matte7.Category = "Sweaters";
            matte7.CreateDate = DateTime.Now;
            matte7.Stock = 10;
            matte7.Size = "XL";
            matte7.Brand = "Ralph Lauren";

            Product matte8 = new Product();
            matte8.Name = "Ralph Lauren Hoodie Blue Lagoon";
            matte8.Price = 1699;
            matte8.Weight = 359;
            matte8.Description = "Huvtröja från Polo Ralph Lauren. Ett bekvämt plagg, tillverkat i en bomull- och polyesterblandning med en mjukt uppruggad insida för ökad komfort och värme.";
            matte8.Image = "RalphLaurenBlueLagoonSweater.jpg";
            matte8.Category = "Sweaters";
            matte8.CreateDate = DateTime.Now;
            matte8.Stock = 10;
            matte8.Size = "L";
            matte8.Brand = "Ralph Lauren";

            Product az5 = new Product();
            az5.Name = "T-Shirt från Polo Ralph Lauren";
            az5.Price = 1495;
            az5.Weight = 200;
            az5.Description = "A two button placket, polo shirt constructed from highly breathable cotton mesh blend which offers a textured look, soft feel and has added stretch for functionality.";
            az5.Image = "tshirtPolo.jpg";
            az5.Category = "T-Shirts";
            az5.CreateDate = DateTime.Now;
            az5.Stock = 12;
            az5.Size = "One Size";
            az5.Brand = "Polo Ralph Lauren";

            Product az6 = new Product();
            az6.Name = "T-Shirt från Hugo Boss";
            az6.Price = 1099;
            az6.Weight = 200;
            az6.Description = "En super soft Polo tröja tillverkad i bomull i slim fit.";
            az6.Image = "tshirt2.jpg";
            az6.Category = "T-Shirts";
            az6.CreateDate = DateTime.Now;
            az6.Stock = 12;
            az6.Size = "One Size";
            az6.Brand = "Hugo Boss";


            Product az7 = new Product();
            az7.Name = "T-Shirt från Daniele Fiesoli";
            az7.Price = 1099;
            az7.Weight = 150;
            az7.Description = "En super soft T-shirt tröja tillverkad i bomull i slim fit.";
            az7.Image = "tshirt3.jpg";
            az7.Category = "T-Shirts";
            az7.CreateDate = DateTime.Now;
            az7.Stock = 12;
            az7.Size = "One Size";
            az7.Brand = "Daniele Fiesoli";




            Product az8 = new Product();
            az8.Name = "T-Shirt från Kenzo";
            az8.Price = 999;
            az8.Weight = 150;
            az8.Description = "Marinblå t-shirt från Kenzo i bomull.";
            az8.Image = "tshirt.jpg";
            az8.Category = "T-Shirts";
            az8.CreateDate = DateTime.Now;
            az8.Stock = 12;
            az8.Size = "One Size";
            az8.Brand = "Kenzo";

            Product az9 = new Product();
            az9.Name = "T-Shirt från Stenströms";
            az9.Price = 999;
            az9.Weight = 150;
            az9.Description = "Marinblå t-shirt från Stenströms i linne.";
            az9.Image = "tshirt4.jpg";
            az9.Category = "T-Shirts";
            az9.CreateDate = DateTime.Now;
            az9.Stock = 12;
            az9.Size = "One Size";
            az9.Brand = "Stenströms";

            Product john1 = new Product();
            john1.Name = "John Henric blå slips";
            john1.Price = 399;
            john1.Weight = 20;
            john1.Description = "En enfärgad slips bör matcha resten av klädseln med tanke på färg och stil men behöver för den skull inte ha samma färg som övrig klädsel. Det viktiga är att slipsen inte skär sig mot övrig klädsel. En enfärgad slips fungerar alltid perfekt till en mörk kostym i blått eller grått, enfärgad vit eller ljusblå skjorta och vitt näsduk.";
            john1.Image = "Blåslips.jpg";
            john1.Category = "Diverse";
            john1.CreateDate = DateTime.Now;
            john1.Stock = 10;
            john1.Size = null;
            john1.Brand = "John Henric";

            Product john2 = new Product();
            john2.Name = "John Henric orange slips";
            john2.Price = 399;
            john2.Weight = 20;
            john2.Description = "Orange bomullsslips från vårt egna varumärke John Henric. Slipsen är handsydd i 100% bomull och kommer i bredd 7 cm. Perfekt till kontoret eller fin middag";
            john2.Image = "Orangeslips.jpg";
            john2.Category = "Diverse";
            john2.CreateDate = DateTime.Now;
            john2.Stock = 10;
            john2.Size = null;
            john2.Brand = "John Henric";

            Product john3 = new Product();
            john3.Name = "John Henric svart slips";
            john3.Price = 399;
            john3.Weight = 20;
            john3.Description = "Detta jämna svarta exemplar går att kombinera vid olika looks och ger varje outfit enkelt och snabbt och fashionabel betoning. Bär den till kontoret eller annat högtidligt evenemang.";
            john3.Image = "svartSlips.jpg";
            john3.Category = "Diverse";
            john3.CreateDate = DateTime.Now;
            john3.Stock = 10;
            john3.Size = null;
            john3.Brand = "John Henric";

            Product john4 = new Product();
            john4.Name = "John Henric brun weekendbag";
            john4.Price = 1299;
            john4.Weight = 200;
            john4.Description = "Den perfekta väskan för längre resor, weekend breaks eller affärsresan. Denna weekend bag är både en rymlig och tålig läderväska. Det kastanjebruna lädret framhävs med metaldetaljer i matt guldfärg och insidan är fodrad med bomullstyg, på insidan finns mindre fack där man med fördel kan förvara mindre ägodelar.";
            john4.Image = "brunväska.jpg";
            john4.Category = "Diverse";
            john4.CreateDate = DateTime.Now;
            john4.Stock = 10;
            john4.Size = null;
            john4.Brand = "John Henric";

            Product john5 = new Product();
            john5.Name = "Moncler ryggsäck";
            john5.Price = 1999;
            john5.Weight = 150;
            john5.Description = "Ryggsäck från Moncler. Ryggsäcken är tillverkad i ett slitstarkt tyg med en läcker design och stilren finish. Insidan av väskan har ett separat, vadderat fack för att tillbehör. Justerbara remmar. Graverad logga på ryggsäckens framsida. ";
            john5.Image = "Monclerväska.jpg";
            john5.Category = "Diverse";
            john5.CreateDate = DateTime.Now;
            john5.Stock = 10;
            john5.Size = null;
            john5.Brand = "Moncler";

            Product john6 = new Product();
            john6.Name = "Valentino ryggsäck";
            john6.Price = 799;
            john6.Weight = 100;
            john6.Description = "Stilren ryggsäck med en enhetlig design som fungerar i alla lägen. Baksida och axelremmar vadderade för bra komfort. Ryggsäcken har ett vadderat fack som rymmer de flesta laptops.";
            john6.Image = "ValentinoRyggsack.jpg";
            john6.Category = "Diverse";
            john6.CreateDate = DateTime.Now;
            john6.Stock = 10;
            john6.Size = null;
            john6.Brand = "Valentino";

            Product john7 = new Product();
            john7.Name = "Lloyd finskor";
            john7.Price = 999;
            john7.Weight = 80;
            john7.Description = "Osmond är en mycket omtyckt klassisk finsko som passar både till kostym och jeans. Ovanlädret är extra mjukt och yttersulan är gjort av en slitstark gummiblandning.";
            john7.Image = "LloydSkorbrun.jpg";
            john7.Category = "Diverse";
            john7.CreateDate = DateTime.Now;
            john7.Stock = 10;
            john7.Size = "45";
            john7.Brand = "Lloyd";

            Product john8 = new Product();
            john8.Name = "Ralph Lauren sneakers";
            john8.Price = 1299;
            john8.Weight = 120;
            john8.Description = "Gant Joree sneakers i skinn. Stilrena och diskreta sneakers med snörning som går ton-i-ton och en lätt yttersula i gummi. Perfekt att matcha till jeans och chinos.";
            john8.Image = "RalphLaurentSkor.jpg";
            john8.Category = "Diverse";
            john8.CreateDate = DateTime.Now;
            john8.Stock = 10;
            john8.Size = "45";
            john8.Brand = "Ralph Lauren";


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
            seedList.Add(az5);
            seedList.Add(az6);
            seedList.Add(az7);
            seedList.Add(az8);
            seedList.Add(az9);
            seedList.Add(tess1);
            seedList.Add(tess2);
            seedList.Add(tess3);
            seedList.Add(tess4);
            seedList.Add(matte1);
            seedList.Add(matte2);
            seedList.Add(matte3);
            seedList.Add(matte4);
            seedList.Add(matte5);
            seedList.Add(matte6);
            seedList.Add(matte7);
            seedList.Add(matte8);
            seedList.Add(product11);
            seedList.Add(product12);
            seedList.Add(product13);
            seedList.Add(product14);
            seedList.Add(product15);
            seedList.Add(product16);
            seedList.Add(product17);
            seedList.Add(product18);
            seedList.Add(product19);
            seedList.Add(john1);
            seedList.Add(john2);
            seedList.Add(john3);
            seedList.Add(john4);
            seedList.Add(john5);
            seedList.Add(john6);
            seedList.Add(john7);
            seedList.Add(john8);

            foreach (var item in seedList)
            {
                _context.Add(item);
            }

            _context.SaveChanges();
        }

    }
}
