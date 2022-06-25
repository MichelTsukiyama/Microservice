# 1. Projeto de Microsserviços

# 2. Sumário
<br>

- [1. Projeto de Microsserviços](#1-projeto-de-microsserviços)
- [2. Sumário](#2-sumário)
- [3. Criar a Solution](#3-criar-a-solution)
- [4. Projeto Catalog.API](#4-projeto-catalogapi)
  - [4.1. Iniciar o projeto](#41-iniciar-o-projeto)
  - [4.2. Adicionar Catalog.API na Solution](#42-adicionar-catalogapi-na-solution)
  - [4.3. Instalar o pacote MongoDB.Driver](#43-instalar-o-pacote-mongodbdriver)
  - [4.4. Criar a classe Product.cs](#44-criar-a-classe-productcs)
  - [4.5. Criar os arquivos de contexto (Acesso ao Mongo e popular banco)](#45-criar-os-arquivos-de-contexto-acesso-ao-mongo-e-popular-banco)
  - [4.6. Criar os arquivos do padrão Repository](#46-criar-os-arquivos-do-padrão-repository)
  - [4.7. Definir a String de Conexão](#47-definir-a-string-de-conexão)
  - [4.8. Registrar os serviços na Classe Startup.cs](#48-registrar-os-serviços-na-classe-startupcs)
  - [4.9. Criar a Controller "CatalogController.cs"](#49-criar-a-controller-catalogcontrollercs)
  - [4.10. Baixar a imagem do MongoDB](#410-baixar-a-imagem-do-mongodb)
  - [4.11. Criar e executar o contêiner do MongoDB](#411-criar-e-executar-o-contêiner-do-mongodb)
  - [4.12. Executando comandos no container MongoDB](#412-executando-comandos-no-container-mongodb)
  - [4.13. Criando o Dockerfile](#413-criando-o-dockerfile)
  - [4.14. Criando o docker-compose](#414-criando-o-docker-compose)
- [5. Projeto Basket.API](#5-projeto-basketapi)
  - [5.1. Criar o projeto](#51-criar-o-projeto)
  - [5.2. Adicionar na Solution](#52-adicionar-na-solution)
  - [5.3. Criar a pasta Entities](#53-criar-a-pasta-entities)
    - [5.3.1. ShoppingCart.cs](#531-shoppingcartcs)
    - [5.3.2. ShoppingCartItem.cs](#532-shoppingcartitemcs)
    - [5.3.3. BasketCheckout.cs](#533-basketcheckoutcs)
  - [5.4. Instalar PAcote NuGet para cache distribuído Redis](#54-instalar-pacote-nuget-para-cache-distribuído-redis)
  - [5.5. Configurando Redis no appsettings.json](#55-configurando-redis-no-appsettingsjson)
  - [5.6. Configurando o Startup.cs](#56-configurando-o-startupcs)
  - [5.7. Criar o diretório Repository](#57-criar-o-diretório-repository)
    - [5.7.1. IBasketRepository](#571-ibasketrepository)
    - [5.7.2. BasketRepository](#572-basketrepository)
  - [5.8. BasketController](#58-basketcontroller)
  - [5.9. Registrar o serviço do Repositório na classe Startup.cs](#59-registrar-o-serviço-do-repositório-na-classe-startupcs)
  - [Criar o Dockerfile de Basket.API](#criar-o-dockerfile-de-basketapi)
  - [Ajustar o Docker-compose para os novos contêiners](#ajustar-o-docker-compose-para-os-novos-contêiners)
    
----

# 3. Criar a Solution

    dotnet new sln --name Microservice

----


# 4. Projeto Catalog.API

## 4.1. Iniciar o projeto
<br>

    dotnet new webapi -o Catalog.API -f net5.0

----

## 4.2. Adicionar Catalog.API na Solution
<br>

    dotnet sln add .\Catalog.API\Catalog.API.csproj

---    

## 4.3. Instalar o pacote MongoDB.Driver
<br>

    dotnet add .\Catalog.API\  package MongoDB.Driver --version 2.16.0

---

## 4.4. Criar a classe Product.cs
<br>
Crie o diretório Catalog.API\Entities e dentro dele a classe Product.cs:

```c#
    public class Product
    {
        // Faz a propriedade ser a chave primária
        // Faz a conversão de ObjectId(MongoDB) para string
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
    }
```

---

## 4.5. Criar os arquivos de contexto (Acesso ao Mongo e popular banco)
<br>

1. Crie o diretório Catolog.API\Data e nele a interface ICatologContext.cs:

```c#
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products{get;}
    }
```

2. Criar a classe CatalogContext.cs:

```c#
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            //As variáveis abaixo vão fazer a conexão com o MongoDB
            var client = new MongoClient(configuration.GetValue<string>
                ("DatabaseSettings:ConnectionString"));

            var database = client.GetDatabase(configuration.GetValue<string>
                ("DatabaseSettings:DatabaseName"));
            
            Products = database.GetCollection<Product>(configuration.GetValue<string>
                ("DatabaseSettings:CollectionName"));

            //Esse método vai popular o banco do MongoDB
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products {get; }
    }
```

3. Criar a classe que vai popular os dados, CatalogContextSeed.cs:

```c#
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            //Verifica se o banco de dados existe
            bool existProduct = productCollection.Find(p => true).Any();
            if(!existProduct)
            {
                //Se não existir, chama o método que insere os dados/popula o banco
                productCollection.InsertManyAsync(GetMyProducts());
            }
        }

        //Método que insere os produtos
        private static IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d214e773f2a3990b47f5",
                    Name = "Iphone X",
                    Description = "Descrição do Iphone",
                    Image = "product-1.png",
                    Price = 950.00m,
                    Category = "Smartphone"
                },
                 new Product()
                {
                    Id = "602d214e773f2a3990b47f6",
                    ...
                }
                //Adicionar outros produtos....
            };
        }
    }
```
----

## 4.6. Criar os arquivos do padrão Repository
<br>

No padrão Repository as entidades de Domínio e a lógica de acesso a dados, se comunicam usando *Interfaces*, isso esconde os detalhes do acesso a dados da camada de negócios.

Inicialmente cria-se uma *Interface* (IProductRepository) que será o contrato que expressa os serviços que a API vai expor, a seguir é implementada em uma classe concreta (ProductRepository).

1. Crie a interface "IProductRepository" no diretório Catalog.API\Repository:

```c#
    public interface IProductRepository
    {
        //Task -> os métodos implementados serão Assíncronos
         Task<IEnumerable<Product>> GetProducts();
         Task<Product> GetProduct(string id);
         Task<IEnumerable<Product>> GetProductByName(string name);
         Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
         Task CreateProduct(Product product);
         Task<bool> UpdateProduct(Product product);
         Task<bool> DeleteProduct(string id);
    }
```

2. Criar a classe "ProductRepository", no mesmo diretório e implementar a interface "IProductRepository":

```c#
 public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged 
                && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .Eq(p => p.Category, categoryName);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .ElemMatch(p => p.Name, name);
            
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(
                filter: g => g.Id == product.Id, replacement: product);
            
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }
    }
```

----

## 4.7. Definir a String de Conexão
<br>

No arquivo "appsettings.json" inserir o seguinte trecho:

```json
{
  "DatabaseSettings":{
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CatalogDb",
    "CollectionName": "Products"
  },
    //restante do código ...
    ...
}
```

----

## 4.8. Registrar os serviços na Classe Startup.cs

Na classe "Startup.cs", adicionar os serviços criados:

```c#
public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            //continuação do código ...
            ...
        }
```

----

## 4.9. Criar a Controller "CatalogController.cs"
<br>

No diretório Catalog.API\Controller criar a classe CatalogController.cs:

```c#
    //Definem a rota padrão e especifica que a classe é uma Controller
    [Route("api/v1/[controller]")]
    [ApiController]
    // É necessário herdar de ControllerBase (não possui a parte de suporte a views do MVC)
    public class CatalogController : ControllerBase
    {
        //Variável e construtor para acesso ao repositório (banco de dados)
        private readonly IProductRepository _repository;

        public CatalogController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        //retorna todos os produtos
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);
        }

        //retorna os produtos por Id(mongo tem id de 24 caracteres)
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetProduct(id);
            if(product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //Retorna os produtos por Categoria
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            if(category is null)
                return BadRequest("Invalid Category");
            
            var products = await _repository.GetProductByCategory(category);
            return Ok(products);
        }

        //Cria um produto novo
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if(product is null)
                return BadRequest("Invalid product");
            
            await _repository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }

        //Atualiza um produto
        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            if(product is null)
                return BadRequest("Invalid product");

            return Ok(await _repository.UpdateProduct(product));
        }

        //Deleta um produto pelo seu ID
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        }
    }
```
---

## 4.10. Baixar a imagem do MongoDB
<br>

    docker pull mongo

----

## 4.11. Criar e executar o contêiner do MongoDB
<br>

    docker run -d -p 27017:27017 --name catalog-mongo mongo

 `-d` - Executa o container em 2 plano, não trava o terminal;

 `-p` - atribui a porta 27017:27017 no container com a mesma porta no host;

 `--name` - atribui o nome "catalog-mongo" ao container;
 
 `mongo` - nome da imagem do MongoDB;

 ---

 ## 4.12. Executando comandos no container MongoDB
 <br>

Essa seção serve somente para verificar e usar um terminal no contêiner do MongoDB

    docker exec -it catalog-mongo /bin/bash

`exec`- executa um comando em um conêiner em execução;

`-it` - aciona o modo iterativo e adiciona um terminal

`/bin/bash` - obtém um shell bash

Comandos para ver as tabelas no banco, criar uma tabela, inserir um registro, deletar um registro e por fim sair do terminal do MongoDB, sair do terminal do contêiner:

    mongo
    show dbs
    use ProductDb
    db.createCollection('Products')
    db.Products.insert({"Name": "Caderno", "Category":"Material Escolar", "Image":"caderno.jpg", "Price":7})
    db.Products.find({}).pretty()
    show dbs
    show collections
    db.Products.remove({})
    db.Products.find({}).pretty()
    exit
    exit

----

## 4.13. Criando o Dockerfile
<br>

Use a extensão do **Docker**, pressione a tecla F1 e digite docker add, surgirão as opções para adicionar os arquivos do docker.

Na sequência, clique com o botão direito do Dockerfile gerado e escolha a opção Build image para criar a imagem.

Ou, crie o Dockerfile, e use o comando abaixo:

    docker build -t catalogapi .

----

## 4.14. Criando o docker-compose
<br>

Também é possível criar o docker-compose usando a extensão, aperte F1 e digite docker add, escolha a opção compose, mas será necessário fazer algumas alterações como acrescentar o container do MongoDB e criar manualmente o docker-compose.orvveride.yml, conforme abaixo:

docker-compose.yml

```Dockerfile
version: '3.4'

services:
  catalogdb:
    image: mongo

  catalogapi:
    image: catalogapi
    build:
      context: .
      dockerfile: Catalog.API/Dockerfile
  
volumes:
  mongo_data:
```

docker-compose.override.yml
```Dockerfile
version: '3.4'

services:
  catalogdb:
    container_name: catalogdb
    restart: always
    ports:
      - "27017:27017"
    volumes:
      - mongo_data:/data/db

  catalogapi:
    image: catalogapi
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - "DatabaseSettings:ConnectionString=mongodb://catalogdb:27017"
    depends_on:
      - catalogdb
    ports:
      - "8000:80" 
```

Para rodar ambos os arquivos use o comando abaixo:

        docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
    
`-f` - indica o target, arquivo a ser lido;
`-d` - rodar em 2º plano, sem travar o terminal;

Esse comando vai iniciar os contêiners, você deve ser capaz de executar os comandos da API. http://localhost:8000/swagger/index.html

----

# 5. Projeto Basket.API
<br>

API em ASPNET Core com banco de dados Redis

----

## 5.1. Criar o projeto
<br>

    dotnet new webapi -o Basket.API -f net5.0

---

## 5.2. Adicionar na Solution
<br>

    dotnet sln add .\Basket.API\

----

## 5.3. Criar a pasta Entities
<br>

No diretório Basket.API crie a pasta Entities, na sequência crie as classes abaixo:

----

### 5.3.1. ShoppingCart.cs
<br>

```c#
public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public ShoppingCart()
        {}

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Qunatity;
                }
                return totalprice;
            }
        }
    }
```

----

### 5.3.2. ShoppingCartItem.cs
<br>

```c#
public class ShoppingCartItem
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
```

----

### 5.3.3. BasketCheckout.cs
<br>

```c#
 public class BasketCheckout
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        //BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        //Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
```

----

## 5.4. Instalar PAcote NuGet para cache distribuído Redis
<br>

    dotnet add .\Basket.API\ package Microsoft.Extensions.Caching.StackExchangeRedis --version 5.0.1

Esse pacote permite a implementação da *Interface* IDistributedcache e seus métodos (Get, Set, Refresh e Remove);

----

## 5.5. Configurando Redis no appsettings.json
<br>

```json
{
  //Adicionar as 2 linhas abaixo  
  "CacheSetting": {
    "ConnectionString": "localhost:6379"
  },
  //restante do código...
  ...
}
```

---

## 5.6. Configurando o Startup.cs
<br>

No método ConfigureServices:

```c#
 public void ConfigureServices(IServiceCollection services)
        {
            //adicionar o bloco abaixo:
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            // restante do código...
            ...
        }
```
---

## 5.7. Criar o diretório Repository
<br>

Crie o diretório Repository com as seguintes classes/Interfaces:

----

### 5.7.1. IBasketRepository
<br>

```c#
    public interface IBasketRepository
    {
         Task<ShoppingCart> GetBasket(string userName);

         Task<ShoppingCart> UpdateBasket(ShoppingCart basket);

         Task DeleteBasket (string userName);
    }
```
-----

### 5.7.2. BasketRepository
<br>

```c#
     public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if(String.IsNullOrEmpty(basket))
                return null;
            
            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

            return await GetBasket(basket.UserName);
        }
    }
```

----

## 5.8. BasketController
<br>

No diretório Controllers criar o controller BasketController:

```c#
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
```

----

## 5.9. Registrar o serviço do Repositório na classe Startup.cs
<br>

```c#
//Acrescentar esta linha no ConfigureServices(IServiceCollection services)
services.AddScoped<IBasketRepository, BasketRepository>();
```

----

## Criar o Dockerfile de Basket.API

Pressione F1, digite docker add e adicione o Dockerfile ao projeto Basket.API

----

## Ajustar o Docker-compose para os novos contêiners

No docker-compose serão adicionados os contêineres do Redis e Basket.API, os arquivos devem ser alterados da seguinte-maneira:


***docker-compose.yml***
```docker
version: '3.4'

services:
  catalogdb:
    image: mongo
  
  basketdb:
    image: redis:alpine

  catalogapi:
    image: catalogapi
    build:
      context: .
      dockerfile: Catalog.API/Dockerfile
  
  basket.api:
    image: basketapi
    build: 
      context: .
      dockerfile: Basket.API/Dockerfile
  
volumes:
  mongo_data:
```

***docker-compose.override.yml***

```docker
version: '3.4'

services:
  catalogdb:
    container_name: catalogdb
    restart: always
    ports:
      - "27017:27017"
    volumes:
      - mongo_data:/data/db
  
  basketdb:
    container_name: basketdb
    restart: always
    ports:
      - "6379:6379"

  catalogapi:
    container_name: catalogapi
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - "DatabaseSettings:ConnectionString=mongodb://catalogdb:27017"
    depends_on:
      - catalogdb
    ports:
      - "8000:80"

  basket.api:
    container_name: basketapi
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - "CacheSettings:ConnectionString=basketdb:6379"
    depends_on:
      - basketdb
    ports:
      - "8001:80"
```

Para subir todos os contêineres use o comando abaixo:

        docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

É possivel acessar a Baskt.API no link: http://localhost:8001/swagger/index.html

É possível acessar o Catalog.API no link: http://localhost:8000/swagger/index.html

----
