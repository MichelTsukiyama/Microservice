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
  - [5.10. Criar o Dockerfile de Basket.API](#510-criar-o-dockerfile-de-basketapi)
  - [5.11. Ajustar o Docker-compose para os novos contêiners](#511-ajustar-o-docker-compose-para-os-novos-contêiners)
- [6. Projeto API.Discount](#6-projeto-apidiscount)
  - [6.1. Criar o Projeto](#61-criar-o-projeto)
  - [6.2. Adicionar na Solution](#62-adicionar-na-solution)
  - [6.3. Inserir os Pacotes Npgsql e Dapper](#63-inserir-os-pacotes-npgsql-e-dapper)
  - [6.4. Criar a pasta Entities](#64-criar-a-pasta-entities)
  - [6.5. Criar a Pasta Repositories](#65-criar-a-pasta-repositories)
    - [6.5.1. Criar Interface IDiscountRepository](#651-criar-interface-idiscountrepository)
    - [6.5.2. Criar a Classe DiscountRepository](#652-criar-a-classe-discountrepository)
  - [6.6. Configurar string de conexão no appsettings.json](#66-configurar-string-de-conexão-no-appsettingsjson)
  - [6.7. Criando DiscountController.cs](#67-criando-discountcontrollercs)
  - [6.8. Configurando a Classe Startup.cs](#68-configurando-a-classe-startupcs)
  - [6.9. Adicionar o Dockerfile](#69-adicionar-o-dockerfile)
  - [6.10. Atualizando os arquivos docker-compose](#610-atualizando-os-arquivos-docker-compose)
    - [6.10.1. docker-compose.yml](#6101-docker-composeyml)
    - [6.10.2. docker-compose.override.yml](#6102-docker-composeoverrideyml)
  - [6.11. Criando a tabela Coupon no PostgreSQL](#611-criando-a-tabela-coupon-no-postgresql)
- [7. gRPC (Google Remote Procedure Call)](#7-grpc-google-remote-procedure-call)
  - [7.1. Diferenças entre API REST e gRPC](#71-diferenças-entre-api-rest-e-grpc)
- [8. Projeto Discount.Grpc](#8-projeto-discountgrpc)
  - [8.1. Criando o Projeto](#81-criando-o-projeto)
  - [8.2. Adicionando na Solution](#82-adicionando-na-solution)
  - [8.3. Instalar os pacotes NuGet (Npgsql e Dapper)](#83-instalar-os-pacotes-nuget-npgsql-e-dapper)
  - [8.4. Copiar a pasta Entities](#84-copiar-a-pasta-entities)
  - [8.5. Copiar a pasta Repositories](#85-copiar-a-pasta-repositories)
  - [8.6. Copiar a ConnectionString](#86-copiar-a-connectionstring)
  - [8.7. Registrar o serviço para o Repositório na Startup.cs](#87-registrar-o-serviço-para-o-repositório-na-startupcs)
  - [8.8. Criar o arquivo Discount.proto](#88-criar-o-arquivo-discountproto)
  - [8.9. Automapper](#89-automapper)
  - [8.10. Criar a classe DiscountService](#810-criar-a-classe-discountservice)
  - [8.11. Atualizar o endpoint na classe Startup:](#811-atualizar-o-endpoint-na-classe-startup)
  - [8.12. Obs. Passos não concluídos](#812-obs-passos-não-concluídos)
    
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

API para cesta de compras em ASPNET Core com banco de dados Redis.

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

## 5.10. Criar o Dockerfile de Basket.API

Pressione F1, digite docker add e adicione o Dockerfile ao projeto Basket.API

----

## 5.11. Ajustar o Docker-compose para os novos contêiners

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

# 6. Projeto API.Discount
<br>

Projeto para criar uma API de Desconto com PostegreSQL e Dapper como ORM.

---

## 6.1. Criar o Projeto
<br>

    dotnet new webapi -o Discount.API -f net5.0

-----

## 6.2. Adicionar na Solution
<br>

    dotnet sln add .\Discount.API\

----

## 6.3. Inserir os Pacotes Npgsql e Dapper

Através do NuGet adicionar os seguintes pacotes:

    dotnet add .\Discount.API\ package Npgsql --version 5.0.14
    dotnet add .\Discount.API\ package Dapper --version 2.0.123

----

## 6.4. Criar a pasta Entities
<br>

Criar o diretório Entities e nele a classe "Coupon.cs":

```c#
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
```

----

## 6.5. Criar a Pasta Repositories
<br>

Criar a pasta Repositories e nele os seguintes arquivos:

--------

### 6.5.1. Criar Interface IDiscountRepository
<br>

```c#
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
```

---

### 6.5.2. Criar a Classe DiscountRepository
<br>

```c#
    //Implementa IDiscountRepository
    public class DiscountRepository : IDiscountRepository
    {
        //Injeção de dependência
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        //Método para acessar o PostgreeSql, usado em todos os outros métodos
        private NpgsqlConnection GetConnectionPostgreSql()
        {
            return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSql();

            //QueryFirstOrDefaultAsync<>() método do Dapper
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", 
                new {ProductName = productName});

            //Se Não existir o Cupom ele cria um novo
            if(coupon is null)
                return new Coupon
                {ProductName = "No Discount", Amount = 0, Description = "No Discount Desc"};

            //Retorna o cumpom (se existir)
            return coupon;
        }


        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSql();

             //ExecuteAsync<>() método do Dapper
            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount});

            if(affected is 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSql();

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName,
                    Description = coupon.Description, 
                    Amount = coupon.Amount});

            if(affected is 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSql();

            var affected = await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName=@ProductName",
                new { ProductName = productName});

            if(affected is 0)
                return false;

            return true;
        }
    }
```

---

## 6.6. Configurar string de conexão no appsettings.json
<br>

No arquivo appsettings.json:

```json
{
  //Adicionar o bloco abaixo  
  "DatabaseSettings": {
    "ConnectionString": "Server=localhost;Port=5432;Database=DiscountDb;User Id=admin;Password=admin1234;"
  },
  //Restante do código...
}
```

----

## 6.7. Criando DiscountController.cs
<br>

No diretório Discount.API/Controllers criar a classe DiscountController:

```c#
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        //Injeção do repositório
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        public async Task<ActionResult<Coupon>> GetDicount(string productName)
        {
            var coupon = await _repository.GetDiscount(productName);
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new {productName = coupon.ProductName}, coupon);
        }

        [HttpPut]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        public async Task<ActionResult<Coupon>> DeleteDiscount(string productName)
        {
            return Ok(await _repository.DeleteDiscount(productName));
        }
    }
```

----

## 6.8. Configurando a Classe Startup.cs
<br>

Na classe Startup.cs incluir o serviço do IDiscountRepository:

```c#
        public void ConfigureServices(IServiceCollection services)
        {
            //Incluir a linha abaixo:
            services.AddScoped<IDiscountRepository, DiscountRepository>();
           //Restante do código...
        }

```

---

## 6.9. Adicionar o Dockerfile
<br>

Usando a extensão do Docker, aperte F1, digite docker add, e crie Dockerfile para o projeto Discount.API.

---

## 6.10. Atualizando os arquivos docker-compose
<br>

Aqui os arquivos serão atualizados para configurar e subir os contêineres da Discount.API, do PostgreSQL e do pgadmin4(ferramenta para administrar o PostegreSQL):

### 6.10.1. docker-compose.yml
<br>

```docker
# Acrescentar em services:
# São as imagens usadas pelo discount.api, postgreSQL e pgadmin4.
services:

  discountdb:
    image: postgres

  pgadmin:
    image: dpage/pgadmin4
    
  discount.api:
    image: discountapi
    build:
      context: .
      dockerfile: Discount.API/Dockerfile

# Acrescentar  em  volumes:
# Esses volumes vão armazenar os dados do PostegreSQL e do pgadmin4.
  volumes:
    
    postgres_data:
    pgadmin_data:
```

----

### 6.10.2. docker-compose.override.yml
<br>

```docker
#Acrescentar os contêiners discountdb, pgadmin e discount.api no services:
services:
  
  discountdb:
    container_name: discountdb
    environment:
      - POSTGRES_USER=admin
      - POSTGRES_PASSWORD=admin1234
      - POSTGRES_DB=Discountdb
    restart: always
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data/

  pgadmin:
    container_name: pgadmin
    environment:
      - PGADMIN_DEFAULT_EMAIL=admin@teste.com
      - PGADMIN_DEFAULT_PASSWORD=admin1234
    restart: always
    ports:
      - "5050:80"
    volumes:
      - pgadmin_data:/root/.pgadmin

  discount.api:
    container_name: discountapi
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - "DatabaseSettings:ConnectionString=Server=discountdb;Port=5432;Database=DiscountDb;User Id=admin;Password=admin1234;"
    depends_on:
      - discountdb
    ports:
      - "8002:80"
```
Utilize o comando abaixo para subir os contêineres:

    docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

----

## 6.11. Criando a tabela Coupon no PostgreSQL
<br>

1. Acesse o contêiner do pgAdmin : http://localhost:5050/login?next=%2F
2. Insira as credenciais que foram configuradas no docker-compose e clique em login:

        admin@teste.com
        admin1234

3. Selecione "Add New Server";
4. Insira em General, o nome ***DiscountServer***;
5. Em Connection coloque o host name/address como ***DiscountDb*** ;
6. Em Connection coloque o username como ***admin*** ;
7. Em Connection coloque o password como ***admin1234*** ;
8. Clique em "Salvar";
9. Acesse o banco de dados criado (DiscountDb)

<center>

![coupon-table](src/Coupon-table.png)

</center>

10. Insira os comandos abaixo:

```sql
CREATE TABLE  Coupon (Id SERIAL PRIMARY KEY,
                     ProductName VARCHAR(24) NOT NULL,
                     Description TEXT,
                     Amount INT);
```
11. Execute os comandos apertando o botão "execute" ou aperte F5;
12. Dê um "refresh" na tabela e veja que a mesma foi criada;
13. (Opcional) Inserir produtos para testar a API:
    
    ```sql
    INSERT INTO Coupon(ProductName, Description, Amount)VALUES('Caderno','Caderno Espiral', 5);
    ```

Agora já é possível ver o produto na tabela tanto pelo pgAdmin quanto pelo método Get da Discount.API;

http://localhost:8002/swagger/index.html (Discount.API)

----

# 7. gRPC (Google Remote Procedure Call)
<br>

É uma arquitetura **RPC** de código aberto criada pelo Google para obter comunicação de alta velocidade entre microsserviços. Permite integração de microsserviços programados em diferentes linguagens.

Usa o formato de mensagem **protobuf**(buffers de protoclo), que fornece um formato de serialização altamente eficiente e com neutralidade de plataforma para serializar mensagens estruturadas que os serviços enviam entre si.

Oferece suporte abrangente entre as pilhas de desenvolvimento mais populares: Java, JavaScript, C#, go, Swift e NodeJS.

As APIs baseadas em RPC são ótimas para ações, ou seja, procedimentos ou comandos. Pode ser uma alternativa mais eficiente do que as APIs Rest

----

## 7.1. Diferenças entre API REST e gRPC
<br>

- Formato de mensagem **Protobuf** ao invés de JSON/XML;
- Construído em **HTTP 2** em vez de HTTP 1.1;
- Gera código nativo em vez de usar ferramentas de terceiros;
- Transmissão de mensagens **muitas vezes mais rápida**;
- **Implementação mais lenta** do que o REST.

> O gRPC ainda não foi amplamente adotado e a maioria das ferramentas de terceiros continua sem recursos para compatibilidade do gRPC.

---

# 8. Projeto Discount.Grpc
<br>

Esse projeto vai construir uma API com padrão gRPC, que utiliza o HTTP2 ao invés do HTTP 1.1 e possui troca de mensagens mais eficientes.

>O .Net implementou o uso do grpc no sdk 3.0

----

## 8.1. Criando o Projeto
<br>

    dotnet new grpc -o Discount.Grpc -F net5.0

-----

## 8.2. Adicionando na Solution
<br>

    dotnet sln add .\Discount.Grpc\
----

## 8.3. Instalar os pacotes NuGet (Npgsql e Dapper)
<br>

    dotnet add .\Discount.Grpc\ package Npgsql --version 5.0.14
    dotnet add .\Discount.Grpc\ package Dapper --version 2.0.123

----

## 8.4. Copiar a pasta Entities
<br>

Copiar a pasta Entities de Discount.API alterando os namespaces para Discount.Grpc

----

## 8.5. Copiar a pasta Repositories
<br>

Copiar a pasta Repositories de Discount.API alterando os namespaces para Discount.Grpc    

----

## 8.6. Copiar a ConnectionString 
<br>

Copiar ConnectionString de Discount.API/appsettings.json para Discount.Grpc/appsettings.json

----

## 8.7. Registrar o serviço para o Repositório na Startup.cs
<br>

```c#
public void ConfigureServices(IServiceCollection services)
        {
            //Adicionar esta linha
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddGrpc();
        }
```
----

## 8.8. Criar o arquivo Discount.proto
<br>

No diretório Discount.Grpc/Protos crie o arquivo "Discount.proto".
Remova o arquivo existente greet.proto;

```proto
syntax = "proto3";

option csharp_namespace = "Discount.Grpc.Protos";

service DiscountProtoService{
    rpc GetDiscount(GetDiscountRequest) returns (CouponModel);

    rpc CreateDiscount(CreateDiscountRequest) returns (CouponModel);
    rpc UpdateDiscount(UpdateDiscountRequest) returns (CouponModel);
    rpc DeleteDiscount(DeleteDiscountRequest) returns (DeleteDiscountResponse);
}

message GetDiscountRequest{
    string productName = 1;
}

message CouponModel{
    int32 id = 1;
    string productName = 2;
    string description = 3;
    int32 amount = 4;
}

message CreateDiscountRequest{
    CouponModel coupon = 1;
}

message UpdateDiscountRequest{
    CouponModel coupon = 1;
}
```

----

## 8.9. Automapper
<br>

1. Instalando o pacote:

    dotnet add .\Discount.Grpc\ package AutoMapper.Extensions.Microsoft.DependencyInjection --version 8.1.1

2. No diretório Discount.Grpc criar a pasta Mapper;
3. Criar a classe DiscountProfile:

```c#
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
```

4. Adicionar Automapper no ConfigureServices na classe Startup.cs:

```c#
        public void ConfigureServices(IServiceCollection services)
        {
            //restante do código...
            //adicionar a linha abaixo:
            services.AddAutoMapper(typeof(Startup));
            //restante do código...
        }
```

----

## 8.10. Criar a classe DiscountService
<br>

No diretório Discount.Grpc/Services criar a classe DiscountServices.cs:

```c#
namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);

            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Discount with ProductName = {request.ProductName} not found."));
            }

            _logger.LogInformation("Discount retrieved for ProductName : {productName}, "
                + "Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repository.CreateDiscount(coupon);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
           var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repository.UpdateDiscount(coupon);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);

            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}
```

---

## 8.11. Atualizar o endpoint na classe Startup:
<br>

Atualizar o endpoint para DiscountService conforme abaixo:

```c#
 app.UseEndpoints(endpoints =>
            {   //Alterar para DiscountService
                endpoints.MapGrpcService<DiscountService>();
                //restante do código
            }
```

----

## 8.12. Obs. Passos não concluídos

Ainda não Descobri como fazer as etapadas do Connected Services(Visual Studio) no visual studio code.

