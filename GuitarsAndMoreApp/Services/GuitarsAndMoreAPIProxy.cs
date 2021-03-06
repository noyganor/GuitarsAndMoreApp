using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GuitarsAndMoreApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Views;
using System.Linq;

namespace GuitarsAndMoreApp.Services
{
    class GuitarsAndMoreAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string CLOUD_PHOTOS_URL = "TBD";
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:30991/GuitarsAndMoreAPI"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.20.76:30991/GuitarsAndMoreAPI"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "http://localhost:30991/GuitarsAndMoreAPI"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:30991/Images/"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.20.76:30991/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:44345/Images/"; //API url when using windoes on development

        private HttpClient client;
        private string baseUri;
        private string basePhotosUri;
        private static GuitarsAndMoreAPIProxy proxy = null;


        public static GuitarsAndMoreAPIProxy CreateProxy()
        {
            string baseUri;
            string basePhotosUri;
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                        basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                        basePhotosUri = DEV_ANDROID_PHYSICAL_PHOTOS_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                    basePhotosUri = DEV_WINDOWS_PHOTOS_URL;
                }
            }
            else
            {
                baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new GuitarsAndMoreAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }

        public string GetPhotoUri()
        {
            return this.basePhotosUri;
        }

        private GuitarsAndMoreAPIProxy(string baseUri, string basePhotosUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
        }

        //Login - if email and password are correct User object is returned. otherwise a null will be returned
        public async Task<User> LoginAsync(string email, string pass)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/Login?email={email}&pass={pass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    User u = JsonSerializer.Deserialize<User>(content, options);
                    //App app = (App)App.Current;               
                    //u.UserFavoritePosts = app.Lookup.UserFavoritePosts;
                    return u;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<User> RegisterUser(User u)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string json = JsonSerializer.Serialize<User>(u, options);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUp", content);
                if (response.IsSuccessStatusCode)
                {

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    User b = JsonSerializer.Deserialize<User>(jsonContent, options);
                    return b;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<LookUpTables> GetLookupsAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetLookUpTables");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    LookUpTables lookup = JsonSerializer.Deserialize<LookUpTables>(content, options);
                    return lookup;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        //check
        public async Task<List<Post>> GetListOfPostsAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetPosts");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<Post> posts = JsonSerializer.Deserialize<List<Post>>(content, options);

                    //Attach lookup objects to each post
                    App app = (App)App.Current;
                    foreach (Post p in posts)
                    {
                        p.Town = app.Lookup.Towns.Where(t => t.TownId == p.TownId).FirstOrDefault();
                        p.Model = app.Lookup.Models.Where(t => t.ModelId == p.ModelId).FirstOrDefault();
                        p.Category = app.Lookup.Categories.Where(t => t.CategoryId == p.CategoryId).FirstOrDefault();
                        p.Producer = app.Lookup.Producers.Where(t => t.ProducerId == p.ProducerId).FirstOrDefault();
                      
                    }
                    return posts;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<Model>> GetListOfModelsAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetModels");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<Model> models = JsonSerializer.Deserialize<List<Model>>(content, options);

                    return models;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> AddPostToUserFavorites(int postID)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/AddPostToFavorites?postID={postID}");
                if (response.IsSuccessStatusCode)
                {

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    bool b = JsonSerializer.Deserialize<bool>(jsonContent, options);
                    return b;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<Post> AddPost(Post p)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };
                string json = JsonSerializer.Serialize<Post>(p, options);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddPost", content);
                if (response.IsSuccessStatusCode)
                {

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Post b = JsonSerializer.Deserialize<Post>(jsonContent, options);
                    if (b != null)
                    {
                        App app = (App)App.Current;
                        b.Town = app.Lookup.Towns.Where(t => t.TownId == b.TownId).FirstOrDefault();
                        b.Model = app.Lookup.Models.Where(t => t.ModelId == b.ModelId).FirstOrDefault();
                        b.Category = app.Lookup.Categories.Where(t => t.CategoryId == b.CategoryId).FirstOrDefault();
                        b.Producer = app.Lookup.Producers.Where(t => t.ProducerId == b.ProducerId).FirstOrDefault();
                    }
                    
                    return b;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Post> EditPost(Post p)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };
                string json = JsonSerializer.Serialize<Post>(p, options);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/EditPost", content);
                if (response.IsSuccessStatusCode)
                {

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Post b = JsonSerializer.Deserialize<Post>(jsonContent, options);
                    if (b != null)
                    {
                        App app = (App)App.Current;
                        b.Town = app.Lookup.Towns.Where(t => t.TownId == b.TownId).FirstOrDefault();
                        b.Model = app.Lookup.Models.Where(t => t.ModelId == b.ModelId).FirstOrDefault();
                        b.Category = app.Lookup.Categories.Where(t => t.CategoryId == b.CategoryId).FirstOrDefault();
                        b.Producer = app.Lookup.Producers.Where(t => t.ProducerId == b.ProducerId).FirstOrDefault();
                    }
                    return b;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<bool> DeletePost(int pId)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.DeleteAsync($"{this.baseUri}/DeletePost?postId={pId}");
                if (response.IsSuccessStatusCode)
                {

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    bool b = JsonSerializer.Deserialize<bool>(jsonContent, options);
                    return b;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<User> UpdateUserDetails(User u)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<User>(u, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UpdateUserDetails", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    User ret = JsonSerializer.Deserialize<User>(jsonObject, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //Upload file to server (only images!)
        public async Task<bool> UploadImage(Models.FileInfo fileInfo, string targetFileName, bool isRegistered )
        {
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.Name));
                multipartFormDataContent.Add(fileContent, "file", targetFileName);
                HttpResponseMessage response = await client.PostAsync($"{this.baseUri}/UploadImage", multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<string> FindGender(int genderId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/FindGender?genderId={genderId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    string genderName = JsonSerializer.Deserialize<string>(content, options);
                    return genderName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> SetUserAsManager(string email)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/SetManager?email={email}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool ok = JsonSerializer.Deserialize<bool>(content, options);
                    return ok;
                }

                else
                    return false;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> IsEmailExistProxy(string email)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/CheckEmailExistance?email={email}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool ok = JsonSerializer.Deserialize<bool>(content, options);
                    return ok;
                }

                else
                    return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return true;
            }
        }
    }

}


