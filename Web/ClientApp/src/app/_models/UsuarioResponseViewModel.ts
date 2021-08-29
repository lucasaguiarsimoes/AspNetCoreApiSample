import { PermissaoSistemaEnum } from "../_enums/PermissaoSistemaEnum";

export class UsuarioResponseViewModel {
  id: number;
  codigo: string;
  nome: string;
  email: string;
  dataHoraUltimaAlteracaoSenha: string;
  expiracaoSenhaAtivada: boolean;
  permissoes: PermissaoSistemaEnum[];
}
