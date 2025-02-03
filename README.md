```markdown
# MinimalApiUsers

Uma API simples e minimalista construída com .NET, PostgreSQL e Swagger para gerenciamento de usuários. Este projeto demonstra como criar e gerenciar usuários em um banco de dados, incluindo operações CRUD, autenticação e documentação da API usando Swagger. Ideal para aprendizado de desenvolvimento full-stack e melhores práticas de APIs.

## Tecnologias Utilizadas

- **.NET 8** - Framework para desenvolvimento de APIs.
- **PostgreSQL** - Banco de dados relacional para armazenamento dos dados de usuários.
- **Swagger** - Ferramenta para documentação da API e testes interativos.
- **Entity Framework Core** - ORM para interação com o banco de dados.
- **Docker (Opcional)** - Para rodar o PostgreSQL em contêineres.

## Como Rodar o Projeto

### Pré-requisitos

1. **.NET SDK**: Certifique-se de ter o [SDK do .NET](https://dotnet.microsoft.com/download) instalado em sua máquina.
2. **PostgreSQL**: Você precisará de uma instância do PostgreSQL em funcionamento. Se preferir, você pode rodar o PostgreSQL via Docker.

### Passos para rodar localmente

1. Clone o repositório:

   ```bash
   git clone https://github.com/NVanitas/MinimalApiUsers.git
   cd MinimalApiUsers
   ```

2. Configure a string de conexão do PostgreSQL no arquivo `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=meubanco;Username=meuusuario;Password=senha"
   }
   ```

3. Realize a migração do banco de dados:

   ```bash
   dotnet ef database update
   ```

4. Rode o projeto:

   ```bash
   dotnet run
   ```

   A API estará disponível em `http://localhost:5000`.

### Testando a API

A API estará documentada com o Swagger, e você pode acessá-la navegando para `http://localhost:5000/swagger` no seu navegador. A partir dessa interface, você poderá testar os endpoints da API interativamente.

### Endpoints

- **POST** `/users` - Criar um novo usuário.
- **GET** `/users` - Obter todos os usuários cadastrados.

## Contribuição

1. Faça um fork do repositório.
2. Crie uma branch para a sua feature (`git checkout -b feature/nova-feature`).
3. Faça o commit das suas alterações (`git commit -am 'Adicionando nova feature'`).
4. Faça o push para a branch (`git push origin feature/nova-feature`).
5. Crie um pull request.

## Licença

Este projeto está sob a [MIT License](LICENSE).
