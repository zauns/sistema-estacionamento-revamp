# sistema-estacionamento-revamp
## Sistema de Estacionamento Avançado - Full Stack .NET
Este repositório documenta a jornada de desenvolvimento de um Sistema de Gerenciamento de Estacionamento completo, construído com as tecnologias mais recentes do ecossistema .NET. O projeto nasceu como um simples aplicativo de console e evoluiu para uma solução robusta e moderna, seguindo uma arquitetura de software limpa e escalável.

Mais do que um projeto finalizado, este é um diário de aprendizado, destacando os desafios enfrentados e as soluções encontradas ao longo do caminho.

### 🚀 Visão do Projeto
O objetivo principal foi transformar uma aplicação básica em um sistema de nível profissional, abrangendo:

- Backend Robusto: Uma API RESTful para gerenciar toda a lógica de negócio.

- Cliente Desktop Nativo: Uma aplicação administrativa em WPF para operações do dia a dia.

- Dashboard Web Moderno: Uma interface web em Blazor para visualização de estatísticas e relatórios.

- Arquitetura Limpa: Separação clara de responsabilidades entre as camadas de Domínio, Infraestrutura, API e Apresentação.

### 🛠️ Stack Tecnológica
- Backend: .NET 8, ASP.NET Core Web API, Entity Framework Core, SignalR

- Banco de Dados: SQLite (para desenvolvimento)

- Frontend Desktop: WPF, Padrão MVVM com CommunityToolkit.Mvvm

- Frontend Web: Blazor Server, MudBlazor, ChartJs.Blazor

- Relatórios: QuestPDF, ClosedXML

### 🌱 Jornada de Aprendizado e Desafios
Cada fase do projeto representou uma nova camada de aprendizado e um conjunto único de desafios.

#### Fase 1 & 2: A Fundação com API e EF Core
A transição de um projeto de console para uma arquitetura de múltiplos projetos foi o primeiro grande passo.

- Metas de Aprendizado:

  - Entender a estrutura de uma Solution .NET com múltiplos projetos (Class Library, Web API).

  - Implementar o Entity Framework Core para mapeamento objeto-relacional.

  - Construir endpoints RESTful e separar a lógica de negócio em uma camada de serviço.

- Dificuldades Superadas:

  - Configuração do Banco de Dados: A escolha inicial do LocalDB se mostrou um desafio ao usar o VS Code. A migração para SQLite simplificou drasticamente o ambiente de desenvolvimento.

  - Refatoração para Camada de Serviço: A lógica inicial no TestController funcionava, mas era difícil de manter. Refatorar para um padrão com IParkingService e ParkingService foi um exercício crucial em design de software e injeção de dependência.

#### Fase 3: Mergulho no Mundo Desktop com WPF
Esta foi a primeira incursão no desenvolvimento de interfaces gráficas com WPF, representando a maior curva de aprendizado.

- Metas de Aprendizado:

  - Compreender o padrão MVVM (Model-View-ViewModel).

  - Utilizar data binding para conectar a UI (View) à lógica (ViewModel).

  - Implementar comandos e gerenciar o estado da UI com o CommunityToolkit.Mvvm.

- Dificuldades Superadas:

  - Erros de Inicialização: Deparar-se com erros como InitializeComponent() não existe e StaticResource not found foi um grande desafio. A solução passou por entender a importância da sincronia entre os ficheiros .xaml e .xaml.cs, especialmente após movê-los para pastas como Views.

  - Comunicação com a API: A aplicação não carregar "silenciosamente" foi um problema frustrante, resolvido ao remover o StartupUri do App.xaml e ao adicionar um tratamento de exceções global para obter mensagens de erro claras.

#### Fase 4 & 5: A Web com Blazor e Funcionalidades Avançadas
Construir o dashboard web trouxe novos desafios, principalmente relacionados à comunicação entre cliente e servidor no ambiente web.

- Metas de Aprendizado:

  - Estruturar uma aplicação Blazor Server e usar bibliotecas de componentes como MudBlazor.

  - Implementar notificações em tempo real com SignalR.

  - Gerar ficheiros dinamicamente (PDF/Excel) a partir da API.

- Dificuldades Superadas:

  - CORS e HTTPS: O desafio mais persistente foi fazer a aplicação Blazor (HTTPS) comunicar com a API (inicialmente em HTTP). A solução exigiu um profundo entendimento de CORS, certificados de desenvolvimento (dotnet dev-certs) e a configuração explícita dos protocolos de escuta (Kestrel) na API.

  - Licenciamento de Bibliotecas: A exceção do QuestPDF foi uma surpresa, mas serviu como um ótimo lembrete da importância de ler a documentação e entender os modelos de licença de pacotes open-source.

### ✅ Progresso do Projeto
#### Este checklist reflete o estado atual do desenvolvimento em comparação com o roadmap original.

- [x] Fase 1: Estruturação Base

- [x] Fase 2: API REST

- [x] Fase 3: Interface Desktop WPF

- [x] Fase 4: Dashboard Web Blazor

- [x] Fase 5: Funcionalidades Avançadas

  - [x] Sistema de Relatórios

  - [x] Exportação de Dados (PDF/Excel)

  - [ ] Notificações em Tempo Real (SignalR)

  - [ ] Cache e Performance

- [ ] Fase 6: Testes e Qualidade

  - [ ] Testes Unitários

  - [ ] Testes de Integração

- [ ] Fase 7: Deploy e DevOps

  - [ ] Containerização com Docker

  - [ ] CI/CD com GitHub Actions
