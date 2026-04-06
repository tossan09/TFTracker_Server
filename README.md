# 🎯 TFT_ANALYZER - Patches & Matches 

## 📌 Sobre
Este repositório contém o **backend** do projeto, desenvolvido em **ASP.NET Core** com integração ao banco de dados **PostgreSQL**.  
O objetivo é fornecer endpoints para listar patches e partidas (matches), que serão consumidos futuramente por um frontend separado.

---

## 🚧 Status
⚠️ **Em desenvolvimento**  
Este backend ainda está em fase inicial.  
- Estrutura de controllers e repositories está sendo construída.  
- Endpoints podem sofrer alterações.  
- Novas funcionalidades serão adicionadas conforme o projeto evolui.

---

## 🛠️ Tecnologias utilizadas
- **C# ASP.NET Core** (Web API)
- **- ADO.NET / Npgsql** para acesso ao PostgreSQL
- **PostgreSQL** como banco de dados

---

## ⚙️ Estrutura básica
- **Controllers**
  - `PatchControllers` → expõe endpoints relacionados a patches.
- **Repository**
  - `PatchRepository` → responsável por consultas SQL e retorno dos dados.
- **Modelos**
  - `Patches` → representa os dados de cada patch (id, patch_number, set_id).

---

## ▶️ Endpoints disponíveis
- `GET /api/patch` → retorna a lista de patches cadastrados.  
- `GET /patch/{patchNumber}` → retorna os matches filtrados por patch específico.  

*(novos endpoints serão adicionados conforme o desenvolvimento avança)*

---

## 📄 Como executar
1. Clone este repositório.
2. Configure a string de conexão com o PostgreSQL no `appsettings.json`.
3. Execute o projeto:
   ```bash
   dotnet run
