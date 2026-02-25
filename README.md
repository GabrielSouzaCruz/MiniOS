# 💻 MiniOS - Simulador de Sistema Operacional

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-Dark_Theme-blue?style=for-the-badge)
![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Status](https://img.shields.io/badge/Status-Concluído-success?style=for-the-badge)

O **MiniOS** é um simulador de Sistema Operacional desenvolvido do zero com interface gráfica (WPF). Este projeto foi construído para demonstrar o funcionamento interno dos principais componentes de um SO real, aplicando conceitos avançados de Arquitetura de Software, Estruturas de Dados e Gerenciamento de Recursos.

<p align="center">
  <img src="MiniOS/Images/logo.png" alt="Logo do MiniOS" width="150"/>
</p>

## 🚀 Principais Funcionalidades (O Motor do Sistema)

### ⚙️ Gestão de Processos (CPU)
* Algoritmo de escalonamento **Round-Robin**.
* Fila de processos dinâmicos (Ready Queue).
* O sistema desconta o tempo de execução (Quantum) e re-insere processos inacabados na fila.

### 🧠 Gestão de Memória (RAM)
* Sistema de alocação e libertação dinâmica de blocos de memória.
* Monitoramento em tempo real com barra de progresso visual que reage à carga (muda de verde para vermelho em situações de *stress* de memória).
* Prevenção de *Out of Memory*: processos só são criados se houver RAM disponível, sendo a memória libertada automaticamente ao fim da execução.

### 🗄️ Sistema de Arquivos Hierárquico (Disco)
* Construído sobre a robusta estrutura de dados **Árvore B+**.
* Suporte nativo a caminhos (Paths) e diretórios virtuais (ex: `Root/Documentos/texto.txt`).
* Operações de Criação, Leitura e Exclusão (*Delete*) com rebalanceamento de dados e busca ultrarrápida (O(log n)).

### ⏱️ Piloto Automático (System Clock)
* Implementação de um *Hardware Timer* (DispatcherTimer) que simula os ciclos da CPU.
* Quando ativado, o Kernel consome a fila de processos e atualiza as interfaces de memória de forma 100% autônoma a cada "batida" do relógio.

## 🏗️ Arquitetura e Padrões de Projeto
O código foi estruturado com foco em boas práticas e separação de responsabilidades:
* **Padrão MVC/Camadas:** Separação estrita entre `Models`, `Services` (Lógica de Negócio), `Controllers` (Orquestração) e `Views` (Interface Gráfica).
* **Padrão Strategy:** Utilizado no escalonador de processos para facilitar a futura adição de novos algoritmos (como FIFO ou SJF) sem alterar o núcleo do Kernel.
* **Redirecionamento de I/O:** O console padrão do C# foi reescrito via `TextWriter` para injetar os logs do Kernel diretamente num terminal virtual dentro do WPF.

## 🏋️‍♂️ Como Executar o Projeto

1. Clone este repositório:
   ```bash
   git clone https://github.com/GabrielSouzaCruz/MiniOS.git
