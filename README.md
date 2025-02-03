```markdown
# Minimal API Users - API de Autenticação com JWT

Este é um projeto de uma API minimalista utilizando **ASP.NET Core**, **Entity Framework** e **JWT Authentication** para gerenciar usuários e autenticação com tokens JWT.

## Funcionalidades

- **Registro de Usuário**: Endpoint para registrar novos usuários.
- **Login (JWT)**: Endpoint para fazer login e gerar um token JWT.
- **Autenticação**: Protege os endpoints com autenticação baseada em JWT.
  
## Tecnologias Utilizadas

- **ASP.NET Core** - Framework para criar a API.
- **Entity Framework Core** - Para o gerenciamento do banco de dados.
- **PostgreSQL** - Banco de dados utilizado para armazenar os usuários.
- **JWT (JSON Web Tokens)** - Para autenticação e autorização na API.
- **Swagger** - Para documentação da API.
  
## Como Executar o Projeto

### Pré-requisitos

- **.NET 8**
- **PostgreSQL** configurado e em execução.
  
### Passos

1. Clone o repositório:

    ```bash
    git clone https://github.com/SEU_USUARIO/MinimalApiUsers.git
    ```

2. Navegue até a pasta do projeto:

    ```bash
    cd MinimalApiUsers
    ```

3. Instale as dependências:

    ```bash
    dotnet restore
    ```

4. Configure a string de conexão no arquivo `appsettings.json`:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=seubanco;Username=seuusuario;Password=suasenha"
      },
      "Jwt": {
        "Key": "supersecretkey12345", 
        "Issuer": "yourIssuer",
        "Audience": "yourAudience"
      }
    }
    ```

5. Crie o banco de dados e aplique as migrações:

    ```bash
    dotnet ef database update
    ```

6. Execute o projeto:

    ```bash
    dotnet run
    ```

### Endpoints da API

#### 1. **POST /register**
Cria um novo usuário.

- **Request Body**:
    ```json
    {
      "name": "Nome do Usuário",
      "email": "email@dominio.com"
    }
    ```

- **Response**:
    - **201 Created**: Usuário criado com sucesso.
    - **400 Bad Request**: Erro na solicitação.

#### 2. **POST /login**
Realiza o login do usuário e gera um token JWT.

- **Request Body**:
    ```json
    {
      "email": "email@dominio.com",
      "password": "senha"
    }
    ```

- **Response**:
    - **200 OK**: Retorna o token JWT.
    - **401 Unauthorized**: Dados incorretos.

#### 3. **Protegido com Autenticação (exemplo)**
Exemplo de como adicionar outros endpoints protegidos:

```csharp
app.MapGet("/protected", [Authorize] () =>
{
    return Results.Ok("Você tem acesso a este endpoint protegido.");
});
```

### Testando com Swagger

O projeto também vem com **Swagger** para testar os endpoints diretamente. Após rodar o projeto, você pode acessar a documentação da API em:

```
http://localhost:5000/swagger
```

### Considerações Finais

- A **chave JWT**, **Issuer** e **Audience** podem ser ajustadas conforme suas necessidades.
- A **validação de senha** no login foi simplificada e deve ser substituída por uma abordagem mais segura (por exemplo, usando **hashing** de senhas).
  
---

Sinta-se à vontade para contribuir com o projeto, sugerir melhorias ou relatar problemas!

## Licença

## Licença

Distribuído sob a **Licença MIT**. Veja o arquivo [LICENSE](LICENSE) para mais informações.
