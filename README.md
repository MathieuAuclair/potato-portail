# Potato intern

Hi new contributor! Make sure to read this readme to facilitate your integration to the project

## VCS Standard

As we use git, we make commit. For the good of everyone, master branch is blocked. To proceed to update master, create a [pull request](https://yangsu.github.io/pull-request-tutorial/). 

As our git flow we use the merge flow (the recommanded by GH), please avoid rebase merge!

When you ask for a PR (pull request), if your PR contains more than 10-20 commits, consider squashing your commit before merging to master. This way all of your commits will be condensed in a single commit. This will ensure that the master branch stays clear for reading.

## Database migration (code first)
To update database, you can create a migration that will update the database

[[Documentation]](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/migrations-and-deployment-with-the-entity-framework-in-an-asp-net-mvc-application)

**IMPORTANT NOTIONS**

If you make any modification to a controller, you need a migration, then you need to database update with PM nuget CLI tool.
**DO NEVER DELETE MIGRATION FILES OR I'LL @#$%?&**&!!!**, by deleting mirgations files, you explode production!

Even if your modification is simple, do a migration. Migration should be considered as commit, just like with git! this imply that the naming of your migration goes with the modification you made.

## Development tools
[MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) use developer edition, it features all tools required, and it's free for development.

# Code Standard
**Language**

All code snippet should be using french as standard as asked by the client.

**HttpRequest Form**

All http request in Controllers should be handled this way

```C#
[HttpPost]
public ActionResult SomeRoute([Required] string idRoute, [Nullable] string someNullable){
   return View();
}
```
the tag ```[Required]``` imply that a request without this parameter should return an error 400 (bad request)

By doing so, we ensure that all request will be the same, and this will avoid stupid time consuming error because of string typos.

**HttpRequest Error**

To suite an error in a ```ResultAction``` make sure to use ASP MVC default response handler like so:

```C#
 return new HttpStatusCodeResult(
     HttpStatusCode.NotFound,
     "Le fichier spécifié est introuvable!"
 );
```

If you are unsure of what error code to use, checkout [Http cat](https://http.cat), all http code are listed. If you want to identify an error code, use the following:

```curl
https://http.cat/[status_code]
```

**Controller logic**

Just to make things clearer, ensure that your Https controllers functions do not contain more than 4 lines of code, if you exceed that, make sure that you feel bad for yourself! The point of this is to clear logic away from http request controller. If you need private function, please! Feel free to add a new HandlerController and inherit that handler to your request handler controller.

> Controller > ControllerHttpRequestHandler > ControllerHttpRequest

**Class definitions**

to make sure we all speak the same language, here's our main class types

* Model                 -> An object found in the database that set standard for content
* Controller            -> The most general definition of an handler, everything inherit from this
* ControllerHandler     -> The object that is responsible for the controllers logic
* HttpRequestController -> The controller that should only contain http request action, inherit from a handler for the logic methods/functions

**Code coverage**

From now on, all new sets of features should include a sets of tests. This is vital as the project is getting bigger, and we want to make sure everything is always working.

**Contributors who won't apply standard**

All pull of your request will be denied!

# Code snippet examples/demos


### Country/Region selector example

[StackBlitz](https://stackblitz.com/edit/geoname-example)

### Converting file
```C#
public string ImageToBase64(Image image,
          System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
```
 
