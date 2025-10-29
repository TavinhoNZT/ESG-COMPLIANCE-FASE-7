Feature: Licenças Ambientais
  Para garantir controle e previsibilidade,
  Como gestora de compliance,
  Quero validar a listagem de licenças e sua estrutura.

  Background:
    Given a base URL configurada

  @happy
  Scenario: Listar licenças com sucesso
    When eu faço GET em "/api/licencas"
    Then o status deve ser 200
    And o corpo deve ser JSON válido
    And o corpo deve ser um array JSON
    And o corpo deve obedecer ao schema "licencas.schema.json"

  @edge
  Scenario: Lista de licenças pode estar vazia
    When eu faço GET em "/api/licencas"
    Then o status deve ser 200
    And o corpo deve ser JSON válido
    And o corpo deve ser um array JSON

  @negative
  Scenario: Rota inexistente deve retornar 404
    When eu faço GET em "/api/licencass"
    Then o status deve ser 404