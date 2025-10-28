Feature: Ações Corretivas
  Para reforçar responsabilidade e melhoria contínua,
  Como responsável operacional,
  Quero validar criação e listagem de ações corretivas.

  Background:
    Given a base URL configurada

  @happy
  Scenario: Criar ação corretiva válida vinculada a auditoria
    Given o payload JSON:
      """
      {
        "auditoriaId": 1,
        "descricao": "Instalação de contenção de efluentes",
        "dataImplementacao": "2030-02-10",
        "responsavel": "Time Operacional"
      }
      """
    Then o payload deve obedecer ao schema "acoes-corretivas.schema.json"
    When eu faço POST em "/api/acoescorretivas" com o payload
    Then o status deve ser 201
    And o corpo deve ser JSON válido

  @negative
  Scenario: Criar ação corretiva inválida (payload incompleto)
    Given o payload JSON:
      """
      {
        "descricao": "",
        "responsavel": "Time Operacional"
      }
      """
    When eu faço POST em "/api/acoescorretivas" com o payload
    Then o status deve ser 400
    And o corpo deve ser JSON válido

  @list
  Scenario: Listar ações corretivas
    When eu faço GET em "/api/acoescorretivas"
    Then o status deve ser 200
    And o corpo deve ser JSON válido
    And o corpo deve ser um array JSON