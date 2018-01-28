using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uimgapi.Models;
using Amazon.S3;
using Microsoft.AspNetCore.Cors;

namespace uimgapi.Controllers
{
    [Produces("application/json")]
    [EnableCors("AllowAll")]
    [Route("api/AwsS3")]
    public class AwsS3Controller : Controller
    {
        private readonly s3uploadtestContext _context;
       // IAmazonS3 S3Client { get; set; }
        S3Objects S3Items;
        private string[] mediaType = {"png","jpeg"};
        public AwsS3Controller(s3uploadtestContext context)
        {
            _context = context;
          //  S3Client = client;
            S3Items = new S3Objects();
        }

        // GET: api/AwsS3
        [HttpGet]
        public IEnumerable<AwsS3> GetAwsS3(string searchString)
        {
            var images = from image in _context.AwsS3
                         select image;

            if (!String.IsNullOrEmpty(searchString))
            {

                images = images.Where(image => image.UniqueCode.Contains(searchString)
                                                || image.Name.Contains(searchString)
                                                || image.Filename.Contains(searchString)
                                                || image.ApprovalStatus.Contains(searchString)
                                                || image.EmailStatus.Contains(searchString)
                                                || image.DateOfBirth.Contains(searchString)
                                                || image.Address.Contains(searchString));
            }

            return images;
        }

        // GET: api/AwsS3/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAwsS3([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var awsS3 = await _context.AwsS3.SingleOrDefaultAsync(m => m.Id == id);

            if (awsS3 == null)
            {
                return NotFound();
            }

            return Ok(awsS3);
        }



        // PUT: api/AwsS3/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAwsS3([FromRoute] long id, [FromBody] AwsS3 awsS3)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awsS3.Id)
            {
                return BadRequest();
            }

            _context.Entry(awsS3).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwsS3Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AwsS3
        [HttpPost]
        public async Task<IActionResult> PostAwsS3([FromBody] dynamic data)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Parse Data
            Newtonsoft.Json.Linq.JToken[] dataArray = ((Newtonsoft.Json.Linq.JContainer)data).ToArray();
            string imageData = "";
            string unquieID = "";
            string name = "";

            foreach (var item in dataArray)
            {
                if (item.Path.ToString().Equals("data"))
                {
                    imageData = item.First.ToString();
                }
                else if (item.Path.ToString().Equals("filename"))
                {
                    name = item.First.ToString();
                }
                else if (item.Path.ToString().Equals("id"))
                {
                    unquieID = item.First.ToString();
                }

            }

            var awsS3 = await _context.AwsS3.SingleOrDefaultAsync(m => m.UniqueCode == unquieID);

            if (awsS3 == null)
            {
                return NotFound();
            }
            string link = S3Items.uploadS3Object(imageData, unquieID);
            if (link != null)
            {
                awsS3.UniqueCode = unquieID;
                awsS3.Filename = name;
                awsS3.UploadedDate = DateTime.Now.ToString("G");
                awsS3.ImageLink = link;
                awsS3.ApprovalStatus = "Pending";
                _context.Entry(awsS3).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwsS3Exists(awsS3.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }   
            return BadRequest();
        }

        private string replaceDataImageString(string base64String)
        {
            string convert = base64String;
            foreach (string item in mediaType)
            {

                if (convert.Contains(string.Format("data:image/{0};base64,", item)))
                {
                    convert = convert.Replace(string.Format("data:image/{0};base64,", item), String.Empty);
                    break;
                }
            }
            return convert;
        }

        // DELETE: api/AwsS3/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAwsS3([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var awsS3 = await _context.AwsS3.SingleOrDefaultAsync(m => m.Id == id);
            if (awsS3 == null)
            {
                return NotFound();
            }

            _context.AwsS3.Remove(awsS3);
            await _context.SaveChangesAsync();

            return Ok(awsS3);
        }

        private bool AwsS3Exists(long id)
        {
            return _context.AwsS3.Any(e => e.Id == id);
        }
    }
}