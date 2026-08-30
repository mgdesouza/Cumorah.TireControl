namespace TireControl.Infrastructure.Data;

public static class AuthorizationPermissions
{
    public const string UsuarioView = "Usuario.View";
    public const string UsuarioCreate = "Usuario.Create";
    public const string UsuarioUpdate = "Usuario.Update";
    public const string UsuarioDelete = "Usuario.Delete";

    public const string PneuView = "Pneu.View";
    public const string PneuCreate = "Pneu.Create";
    public const string PneuUpdate = "Pneu.Update";
    public const string PneuDelete = "Pneu.Delete";

    public const string VeiculoView = "Veiculo.View";
    public const string VeiculoCreate = "Veiculo.Create";
    public const string VeiculoUpdate = "Veiculo.Update";
    public const string VeiculoDelete = "Veiculo.Delete";

    public const string RelatorioView = "Relatorio.View";

    public static readonly IReadOnlyCollection<string> All =
    [
        UsuarioView, UsuarioCreate, UsuarioUpdate, UsuarioDelete,
        PneuView, PneuCreate, PneuUpdate, PneuDelete,
        VeiculoView, VeiculoCreate, VeiculoUpdate, VeiculoDelete,
        RelatorioView
    ];
}
