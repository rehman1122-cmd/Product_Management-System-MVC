# Elite Watches

A small ASP.NET Core MVC project for practicing the Model-View-Controller pattern and basic CRUD operations. The watch catalog is stored locally in SQLite.

## Run the project

1. Install the .NET 10 SDK.
2. Open a terminal in the `Lab 9` folder.
3. Run:

   ```sh
   dotnet run
   ```

4. Open the URL printed in the terminal (the launch profile uses `http://localhost:5080`).

The app creates `App_Data/elite-watches.db` on first run. `Data/schema.sql` creates the `Products` table and inserts sample watches into an empty catalog. Delete the database file to reset the sample data.

## How the MVC pieces fit together

- **Model:** `Models/Product.cs` defines watch data and validation rules.
- **View:** Razor pages in `Views/` display the collection and CRUD forms.
- **Controller:** `Controllers/ProductsController.cs` receives page requests and returns the appropriate view.
- **Data access:** `Repositories/IProductRepository.cs` defines the storage contract, and `Data/ProductRepository.cs` runs parameterized SQLite queries.
- **Startup:** `Program.cs` registers dependencies and initializes the SQLite database from the schema script.

The main workflow is `Browser → ProductsController → IProductRepository → SQLite`, then the controller passes a model back to a Razor view to render HTML.

## Pages

- `/` — project introduction and MVC overview
- `/Products` — list watches
- `/Products/Create` — add a watch
- `/Products/Details/{id}` — view one watch
- `/Products/Edit/{id}` — update a watch
- `/Products/Delete/{id}` — confirm and delete a watch
