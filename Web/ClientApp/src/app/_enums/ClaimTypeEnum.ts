/**
 * Custom ClaimTypes
 */
export enum ClaimTypeEnum {
  Sub = 'sub', // Representa o dono do token
  Jti = 'jti', // Identificador único do token
  Role = 'Role', // Representa claims de permissões disponíveis
}
