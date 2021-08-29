import { PermissaoSistemaEnum } from "../_enums/PermissaoSistemaEnum";

export interface UsuarioEditViewModel {
  id: number;
  codigo: string;
  nome: string;
  email: string;
  senha: string | null;
  expiracaoSenhaAtivada: boolean;
  permissoes: PermissaoSistemaEnum[];
}
