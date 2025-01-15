# Projeto de Consulta e Adição de Melhores Rotas

Este projeto é uma aplicação .NET 8 utilizando Entity Framework 9 e banco de dados SQLite para adicionar e consultar rotas com base no custo. A estrutura do projeto segue o padrão arquitetural 
**Data Driven** e foi organizada em três principais pastas:

- **Application**: Contém os serviços da aplicação.
- **Domain**: Contém as entidades de domínio.
- **Infrastructure**: Contém o contexto do banco de dados e implementações de infraestrutura.

## Algorítimo Guloso
Algorítmo Guloso foi utilizado para encontrar a rota com menor custo.
De modo geral, algoritmos gulosos são usados em problemas de otimização onde é interessante realizar um conjunto de melhores soluções locais, possuindo como objetivo obter uma solução ótima global, tomando como base um determinado parâmetro. Os parâmetros que definem o que será a melhor solução durante a iteração irá variar para cada problema.

## Estrutura do Projeto

O projeto é dividido nas seguintes pastas:

### 1. **Application**
Contém os serviços responsáveis pelas regras de negócio e interação com o banco de dados. Aqui estão implementadas as lógicas para consultar e adicionar rotas de acordo com o custo.

- **RouteService.cs**: Serviço responsável pela lógica de adição e consulta das rotas.

### 2. **Domain**
Contém as entidades de domínio que representam os principais objetos do sistema.

- **Route.cs**: Representa a entidade de uma rota, incluindo os campos como origem, destino, custo e outros atributos.

### 3. **Infrastructure**
Contém o contexto do banco de dados e configurações de acesso.

- **SQLiteDbContext.cs**: Contexto do Entity Framework para o banco de dados SQLite.
- **RouteRepository.cs**: Repositório responsável pelas operações CRUD para a entidade de rota no banco de dados.

## Tecnologias Utilizadas

- .NET 8
- Entity Framework 9
- SQLite

## Como Executar o Projeto

Siga os passos abaixo para executar o projeto em sua máquina local.

### 1. **Clone o Repositório**

Clone este repositório para a sua máquina local usando o comando:

```bash
git clone https://github.com/clt-pereira/best-route.git
```

### 2. **Restaurar os Pacotes NuGet**

Navegue até o diretório do projeto e execute o comando para restaurar os pacotes NuGet:

```bash
cd best-route
dotnet restore
```

Este comando irá baixar todas as dependências necessárias para o projeto.

### 3. **Configurar o Banco de Dados**

O projeto utiliza o banco de dados SQLite e o Banco de Dados assim como as suas tabelas serão criadas automaticamente durante a execução.

### 4. **Executar a Aplicação**

Como este projeto é uma aplicação do tipo console, você pode executá-lo utilizando o comando abaixo:

```bash
dotnet run
```

### 5. **Interação com o Sistema**

Após rodar o programa, você poderá adicionar novas rotas e consultar as rotas existentes. O sistema irá organizar as rotas de acordo com o custo. Você será guiado por prompts interativos na linha de comando para realizar as ações.

- **Adicionar uma rota**: Você será solicitado a fornecer a origem, destino e custo da rota.
- **Consultar rotas**: Após adicionar as rotas, você poderá consultá-las, e elas serão ordenadas pelo menor custo.

## Funcionalidades

- **Adicionar uma nova rota**: A aplicação permite adicionar rotas especificando a origem, o destino, e o custo associado.
- **Consultar rotas**: Você pode consultar rotas já registradas no banco de dados, com a possibilidade de ordená-las de acordo com o custo.

## Como Funciona

A aplicação segue um padrão Data Driven, onde as rotas são adicionadas ao banco de dados e consultadas através de um serviço que interage diretamente com o contexto do banco de dados (via Entity Framework). A consulta de rotas é feita com base no custo, permitindo que o usuário encontre as melhores opções.

### Exemplo de Uso

1. Ao rodar a aplicação, ela solicitará ao usuário para adicionar rotas com os seguintes campos:
   - **Origem**: Cidade ou local de origem.
   - **Destino**: Cidade ou local de destino.
   - **Custo**: Custo associado à rota.

2. O usuário pode então consultar todas as rotas ou filtrar as rotas com base na origem e destino informados.
   - **Origem**: Local de origem.
   - **Destino**: Local de destino.