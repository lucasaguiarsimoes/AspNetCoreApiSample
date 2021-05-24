
# API de Exemplo em ASP.NET Core
Solution com uma API em ASP NET Core com as seguintes funcionalidades:
 
## 1) Rota de geração de token/autenticação (Login)
Descrição: Rota na API que recebe um usuário e senha e retorna um token JWT com as permissões desse usuário;
O usuário do sistema tem um controle de expiração em que, se ativado, sua senha será expirada e deverá ser redefinida após um período de 3 meses após a última troca de senha;

### 1.1) EntityFrameworkCore
Descrição: O sistema utiliza um banco de dados SQLServer acessível via EntityFrameworkCore e inicia com um usuário e senha 'admin' para o primeiro acesso;

### 1.2) Permissões (Roles) que o sistema exigirá em outras rotas de acesso
Descrição: O sistema possui roles específicas para acesso às rotas de CRUD;

## 2) Rotas de CRUD gerenciamento de usuários;
* Rota para consulta de usuários e suas permissões, ou seja, usuários existentes;
* Rota para criação de novos usuários já com suas eventuais permissões;
* Rota para edição de usuários e suas permissões;
* Rota para remoção de usuários e consequentemente suas permissões;
 
**Observação:** O sistema exige que o usuário possua permissão para acionamento às rotas de CRUD através do token JWT;

## 3) Conceitos e Recursos utilizados:
1) Dependency Injection: Para injetar dependências, por exemplo, o service que realiza as ações (ex: UsuarioService);
2) Padrão MVVM: Objetos recebidos são ViewModels e são convertidos pra objetos de domínio;
3) AutoMapper: utilizado para conversão de DTO (Data-transfer-objects);
4) RoleBasedAuthentication: Utilizado para garantir o acesso às rotas da API;
5) Pattern Repository: Utilizado na camada de serviço para abstrair a fonte de dados do sistema;
6) Patter UnitOfWork: Utilizado para centralizar os diferentes repositórios e a lógica de acesso à base de dados;
7) Permissões específicas utilizadas para cada operação do CRUD: Consultar, Inserir, Editar e Apagar;
8) Swagger para acionamento da API;
9) Testes unitários com XUnit e NSubstitute;
