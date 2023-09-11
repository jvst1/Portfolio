# Sistema de gestão de pedidos

---

## Objetivo

### Introdução

O Sistema de Gestão de Pedidos pretende revolucionar a forma como as lojas locais gerem os seus pedidos. Ao integrar este sistema, as lojas locais podem aceder a uma rede de clientes totalmente nova e agilizar o processamento de pedidos.

---

## O projeto

### Quem é o cliente?

Lojas locais que buscam expandir sua base de clientes e agilizar seus processos de pedidos.

### Que problemas ou oportunidades resolveremos?

- Alcance limitado do cliente para lojas locais.
- Gestão ineficiente de pedidos.
- Falta de rastreamento e notificações em tempo real.

### Qual é o benefício que os clientes podem obter?

- Acesso a uma base de clientes mais ampla.
- Gerenciamento simplificado de pedidos.
- Maior satisfação do cliente por meio de rastreamento e notificações em tempo real.

### O que o cliente deseja ou precisa?

- Uma interface fácil de usar para gerenciar pedidos.
- Um sistema confiável e escalável que pode lidar com o crescimento.

### Como será a experiência do cliente ao usar este novo serviço?

Os clientes acharão o sistema intuitivo e fácil de usar. Eles serão capazes de gerenciar pedidos com eficiência prestando um melhor serviço aos seus clientes.

---

## Metodologia

### Como o trabalho foi organizado?

O trabalho foi organizado utilizando uma abordagem Kanban, o que permitiu maior flexibilidade e foco na entrega contínua de valor. O quadro Kanban foi dividido em várias colunas, como "Novo", "Backlog", "Em desenvolvimento", "Em revisão" e "Feito", para rastrear o status de várias tarefas.

#### Fase inicial: Arquitetura e documentação
A primeira grande entrega foi estrategicamente focada em estabelecer uma base sólida para o projeto. Isso incluiu:

- **Documentação**: Documentação abrangente foi criada para descrever a arquitetura do sistema, os requisitos funcionais e não funcionais e os casos de uso.
  
- **Arquitetura**: O backend foi desenvolvido utilizando arquitetura Domain-Driven Design (DDD) em C# com dotnet 6.0. Essa abordagem ajudou na criação de uma base de código com possibilidade de escalonamento e sustentável. O padrão Repository foi implementado usando Entity Framework Core para gerenciar operações de dados, garantindo uma separação clara de interesses.

#### Quality Assurance:
Para manter a alta qualidade, cada tarefa precisava atender a determinados critérios antes de passar para a próxima coluna do quadro Kanban. Code review e testes unitários eram partes essenciais do fluxo de trabalho.

- **Teste Unitário**: Os testes unitários foram implementados usando xUnit e a simulação foi feita usando Moq. Isso garantiu que cada componente fosse testado isoladamente, tornando a base de código mais robusta e mais fácil de manter.

### Diagrama de caso de uso:

#### Para o usuário final:
- Navegue pelos produtos: Permite que os usuários finais visualizem uma lista de produtos disponíveis. Recursos como filtragem e classificação podem estar disponíveis para ajudar os clientes a encontrar produtos com mais facilidade.
- Faça a encomenda: Permite que os usuários finais selecionem produtos, adicionem-nos ao carrinho de compras e façam um pedido. Durante este processo, os clientes podem especificar os detalhes da entrega e confirmar o pedido.
- Acompanhar Pedido: Fornece aos usuários finais atualizações em tempo real sobre o status de seus pedidos. Isso pode incluir etapas como “Processamento”, “Saída para entrega” e “Entregue”.
- Ver histórico de pedidos: permite que os usuários finais visualizem uma lista de todos os pedidos anteriores. Esse recurso também pode permitir que os clientes façam novos pedidos de itens de seu histórico.

#### Para o administrador:

- Navegar pelos produtos: Permite que o administrador visualize uma lista de produtos disponíveis. Recursos como filtragem e classificação podem estar disponíveis para ajudar o administrador a encontrar produtos com mais facilidade.
- Cadastrar novos produtos: Permite que o administrador cadastre novos produtos para o sistema.
- Atualizar produtos: Permite que o administrador atualize os produtos existentes no sistema.
- Deletar produtos: Permite que o administrador remova produtos do sistema.

![Diagrama de caso de uso](Portfolio.Documentos/UseCase.png)

### Requisitos Funcionais

#### Requisitos Funcionais para Clientes

| Código | Identificação | Classificação | Ator | Objetivo |
| --- | --- | --- | --- | --- |
| RF001 | Navegar Produtos  | Essencial | Cliente | Visualizar uma lista de produtos disponíveis |
| RF002 | Realizar Pedido   | Essencial | Cliente | Adicionar produtos ao carrinho e confirmar o pedido |
| RF003 | Consultar Pedido   | Essencial | Cliente | Visualizar informações do pedido em tempo real |
| RF004 | Ver Histórico de Pedidos| Essencial | Cliente | Visualizar e possivelmente refazer pedidos anteriores |

---

#### Requisitos Funcionais para Administradores do Sistema

| Código | Identificação | Classificação | Ator | Objetivo |
| --- | --- | --- | --- | --- |
| RF005 | Gerenciar produtos | Essencial | Administrador do Sistema | Adicionar, remover ou atualizar produtos |
| RF006 | Ver Métricas do Sistema | Essencial | Administrador do Sistema | Visualizar análises relacionadas a pedidos e vendas |

---

#### Requisitos Não Funcionais

| Código | Identificação | Classificação | Objetivo |
| --- | --- | --- | --- |
| RNF001 | Escalabilidade | Essencial | O sistema deve ser escalável horizontalmente |
| RNF002 | Disponibilidade | Essencial | O sistema deve estar disponível 24/7 |
| RNF003 | Redundância | Essencial | Os dados devem ser replicados para tolerância a falhas |
| RNF004 | Segurança | Essencial | Transmissões de dados criptografadas, autenticação forte |
| RNF005 | Desempenho | Essencial | Baixa latência para todas as operações |
| RNF006 | Usabilidade | Essencial | Interface de usuário intuitiva e multiplataforma |
| RNF007 | Recuperação de Desastres | Essencial | Backups regulares e processo de recuperação bem definido |
| RNF008 | Manutenibilidade | Essencial | Fácil de atualizar e bem documentado |
| RNF009 | Conformidade | Essencial | Conformidade com regulamentações legais como o GDPR|


### Diagrama C4 - Contexto

![Diagrama C4 - Contexto](Portfolio.Documentos/C4-Contexto.png)

### Diagrama C4 - Container

![Diagrama C4 - Container](Portfolio.Documentos/C4-Container.png)

---

## Tecnologias utilizadas

- **Backend**: C#
- **Frontend**: Vue.js
- **Notification Service**: [Zenvia](https://www.zenvia.com)
- **Database**: SQL Server [AWS RDS](https://aws.amazon.com/pt/rds/?p=ft&c=db&z=3)
- **Frontend Hosting**: [Vercel](https://vercel.com)
- **Backend Hosting**: Amazon Elastic Container Service [AWS ECS](https://aws.amazon.com/pt/ecs/?nc2=h_ql_prod_ct_ecs)

---

## Cronograma do Projeto

| Semana | Entrega Prevista | Descrição |
| --- | --- | --- |
| 1-4 | Documentação | Criação da documentação inicial e planejamento do projeto |
| 4-7 | Desenvolvimento Backend | Implementação dos serviços e APIs do backend em C# |
| 7-9 | Desenvolvimento Frontend | Desenvolvimento da interface do usuário em Vue.js |
| 10 | Testes | Testes unitários com xUnit e Moq |
| 11 | Implantação | Implantação do sistema em ambiente de produção |
| 12-13 | Revisão | Revisão e ajustes finais |
| 14 | Entrega Final | Entrega do projeto e documentação final |


---

## Escopo do projeto

O escopo inclui o desenvolvimento de um sistema de gerenciamento de pedidos baseado na web com rastreamento e notificações em tempo real. Será escalonável, seguro e fácil de usar.

---

## Contexto

O projeto é voltado para lojas locais que desejam ampliar sua base de clientes e agilizar seus processos de pedidos.

---
