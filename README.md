# 📝 Bl.Financial.Size.Sample

Projeto para antecipação de recebíveis, sendo desenvolvido uma API ASP NET Core para o gerenciamento dos dados.

## 🚀 Sobre o Projeto

Gerenciamento de antecipação de recebíveis de NFs, podendo ter N empresas para N notas fiscais.

## 📋 Funcionalidades

- Consulta e inserção de empresa
- Consulta e inserção de nota fiscal (NF)
- Inserção de NF para o carrinho de antecipação
- Remoção de NF para o carrinho de antecipação
- Consulta dos dados da empresa e dados do carrinho de antecipação

## 🛠️ Tecnologias Utilizadas
![.NET 8.0](https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server 16.0](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

### ⚙️ Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) ou [Docker](https://www.docker.com/)
- [Git](https://git-scm.com/)

## 📦 Instalação

Realize os seguintes passos para utilizar o projeto.

#### Clone o repositório
```bash
git clone https://github.com/GuilhermeBley/Bl.Financial.Size.Sample.git
```

#### Caminhe até a pasta do projeto
```bash
cd src/Bl.Financial.Size.Sample.Server
```

#### Insira as connection strings
Verifique a `connection string` do seu banco SQL Server e troque pelo valor `<sql_connectionstring>` no seguinte comando: 
```bash
dotnet user-secrets set "ConnectionStrings:SqlServer" "<sql_connectionstring>"
```

ou vá até o arquivo `appsettings.json` e substitua o valor do campo `ConnectionStrings:SqlServer`

#### Restaure as dependências
```bash
dotnet restore
```

#### Execute o projeto
```bash
dotnet run --launch-profile Development
```

#### Accese os endpoints

- Utilize o swagger
*ou*
- Utilize diretamente o método GET `https://localhost:7194/api/anticipation/company/cnpj/00000000000000`