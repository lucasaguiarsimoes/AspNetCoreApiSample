import { PermissaoSistemaEnum } from "../_enums/PermissaoSistemaEnum";

export interface UsuarioAddViewModel {
  codigo: string;
  nome: string;
  email: string;
  senha: string;
  expiracaoSenhaAtivada: boolean;
  permissoes: PermissaoSistemaEnum[];
}
