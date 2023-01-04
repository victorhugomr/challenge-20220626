# Backend Challenge 20220626 😎

Este repositório contém os códigos do projeto que demonstram a integração C# com o [MongoDB Atlas](https://www.mongodb.com/atlas/database).

A aplicação consiste em realizar um _Web Scraping_ através do desencadeador agendado pelo sistema do Cron. A integração é feita através de uma REST API para realizar as requisições GET, e utilizando o banco de dados em nuvem MongoDB Atlas para armazenar os dados extraídos da página [Open Food Facts](https://world.openfoodfacts.org/).

Este desafio foi realizado pela Coodesh como teste técnico para vaga de Backend .NET/C# Developer (Júnior).
>  This is a challenge by [Coodesh](https://coodesh.com/)


## Usando o Projeto

A REST API foi desenvolvida na linguagem C# e utiliza o .NET 7.0. Para utilizá-la, você pode analisar o melhor jeito de instalar de acordo com seu ambiente de desenvolvimento. No seguinte projeto foi utilizado o [Visual Studio Code](https://code.visualstudio.com/).

Você pode instalá-lo a partir do Visual Studio Code procurando por 'C#' na aba Extensões (Ctrl+Shift+X), no canto superior esquerdo, ou se você já tiver um projeto com arquivos C#, o Visual Studio Code solicitará que você instale a extensão assim que abrir um arquivo C#.

Para clonar o repositório, você também pode executar cada amostra diretamente da linha de comando no Terminal.

## Clonando o projeto através da linha de comando
* Clone este repositório:
```
    $ git clone https://github.com/victorhugomr/challenge-20220626.git
```
* Include the [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/):
```
    PM> Install-Package AuthorizeNet
```
* Include the [HtmlAgilityPack](https://html-agility-pack.net/):
```
    PM> dotnet add package HtmlAgilityPack --version 1.11.46
```
* Include the [Quartz](https://www.quartz-scheduler.net/):
```
    PM> dotnet add package Quartz --version 3.5.0
```
* Include the [Microsoft.AspNetCore.OpenApi](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-7.0):
```
    PM> dotnet add package Microsoft.OpenApi --version 1.4.5
```
* Include the [Swashbuckle.AspNetCore](https://learn.microsoft.com/pt-br/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio):
```
    PM> dotnet add package Swashbuckle.AspNetCore --version 6.4.0
```
 Faça o _build_ do projeto para produzir a REST API da aplicação web.
* Execute a seguinte amostra no Terminal para iniciar:
```
     > dotnet run
```

## Inicializando o projeto através do Docker 🐳
* Instale o Docker adequado ao seu sistema em [https://www.docker.com/](https://www.docker.com/).

* Após o clone do repositório e da instalação do Docker, execute o Docker. Utilizar a extensão Docker no VS Code facilita a visualização.

* No Terminal, dentro do diretório clonado, execute o seguinte comando:
```
    PM> docker run -d -p 5000:80 --name challenge-20220626 challenge-20220626
```
-{port}: será a porta que você poderá acessar através do localhost (ex: http://localhost:5000/);
-{name}: será o nome do container a ser executado;
-{image} será a imagem utilizada para a criação do container.
```
    PM> docker run -d -p {port}:80 --name {name} {image}
```

Após a execução, estarão disponíveis para visualização no navegador, a partir do localhost, as seguintes rotas HttpGet:

* **/**: Retorna um Status: 200 e uma Mensagem "Fullstack Challenge 20201026";
* **/products/**: Lista todos os produtos da base de dados;
* **/products/:_code_**: Obtém a informação de um produto através do _code_ do produto.
