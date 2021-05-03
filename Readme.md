
# API em ASP.NET Core
Solution com uma API em ASP NET Core com as seguintes funcionalidades:
 
## 1) Rota de geração de token/autenticação (Login)
Descrição: Rota na API que recebe um usuário e senha e retorna um token JWT com as permissões desse usuário;

### 1.1) Lista de usuários inicial hardcoded
Descrição: O sistema não possui um banco de dados real, o foco desse sistema de exemplo é a própria API

### 1.2) Permissões (Roles) que o sistema exigirá em outras rotas de acesso
Descrição: O sistema possui roles específicas para acesso às rotas de CRUD

## 2) Rotas de CRUD gerencialmento de usuários;
* Rota para consulta de usuários e suas permissões, ou seja, usuários existentes;
* Rota para criação de novos usuários já com suas eventuais permissões;
* Rota para edição de usuários e suas permissões;
* Rota para remoção de usuários e consequentemente suas permissões;
 
**Observação:** Garantir que o usuário possui permissão para acionamento da rota (a partir do token);
**Informações gerais:** O sistema pode começar com uma lista estática de usuários para realização de login e obtenção do token. As rotas de CRUD podem manipular essa lista simulando uma tabela de banco de dados.

## 3) Detalhes:
1) Dependency Injection para injetar dependências, por exemplo, o service que realiza as ações (ex: UsuarioService);
2) Objetos recebidos são ViewModels e são convertidos pra objetos de domínio;
3) AutoMapper utilizado para conversão de DTO (Data-transfer-objects);
4) RoleBasedAuthentication utilizado para garantir o acesso às rotas da API;
5) Permissões específicas utilizadas para cada operação do CRUD: Consultar, Inserir, Editar e Apagar;
6) Uso do pattern IRepository na camada de serviço para abstrair a fonte de dados do sistema;
7) Swagger para acionamento da API;