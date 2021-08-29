export enum PermissaoSistemaEnum {
  UsuarioConsulta = 1,
  UsuarioCria = 2,
  UsuarioEdita = 3,
  UsuarioRemove = 4
}

export const PermissaoSistemaEnumLabel = new Map<PermissaoSistemaEnum, string>([
  [PermissaoSistemaEnum.UsuarioConsulta, 'Consulta de Usuários'],
  [PermissaoSistemaEnum.UsuarioCria, 'Criação de Usuários'],
  [PermissaoSistemaEnum.UsuarioEdita, 'Edição de Usuários'],
  [PermissaoSistemaEnum.UsuarioRemove, 'Exclusão de Usuários'],
]);
