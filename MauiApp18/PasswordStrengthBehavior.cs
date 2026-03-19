using Org.Apache.Http.Authentication;

namespace MauiApp18;
/********************************************
nazwa funkcji: PasswordStrengthBehavior
opis funkcji: Behavior podłączany do Entry, który ocenia siłę hasła na podstawie długości, obecności cyfry i wielkiej litery. Wynik wyświetlany jest w powiązanym Labelu.
parametry: StrengthLabel – etykieta, w której wyświetlana jest siła hasła
zwracany typ i opis: brak – Behavior nie zwraca wartości, działa reaktywnie
autor:           
********************************************/
public class PasswordStrengthBehavior : Behavior<Entry>
{
    public static readonly BindableProperty StrengthLabelProperty =
        BindableProperty.Create(nameof(StrengthLabel), typeof(Label), typeof(PasswordStrengthBehavior));

    public Label StrengthLabel
    {
        get => (Label)GetValue(StrengthLabelProperty);
        set => SetValue(StrengthLabelProperty, value);
    }
    /********************************************
    nazwa funkcji: OnAttachedTo
    opis funkcji: Metoda wywoływana podczas podłączania Behavior do kontrolki Entry. Rejestruje obsługę zdarzenia TextChanged, aby reagować na zmiany hasła.
    parametry: entry – kontrolka Entry, do której Behavior zostaje podłączony
    zwracany typ i opis: void – metoda nie zwraca wartości
    autor: 
    ********************************************/
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }
    /********************************************
    nazwa funkcji: OnDetachingFrom
    opis funkcji: Metoda wywoływana podczas odłączania Behavior od kontrolki Entry. Usuwa obsługę zdarzenia TextChanged, aby uniknąć wycieków pamięci.
    parametry: entry – kontrolka Entry, do której Behavior zostaje odłączony
    zwracany typ i opis: void – metoda nie zwraca wartości
    autor: 
    ********************************************/
    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }
    /********************************************
    nazwa funkcji: OnEntryTextChanged
    opis funkcji:  Obsługuje zdarzenie TextChanged. Odczytuje nowe hasło, ocenia jego siłę i aktualizuje Label odpowiednim tekstem oraz kolorem.
    parametry:  sender – obiekt wywołujący zdarzenie (Entry), e – argumenty zdarzenia zawierające nowy tekst
    zwracany typ i opis: void – metoda nie zwraca wartości
    autor: 
    ********************************************/
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        string password = e.NewTextValue ?? string.Empty;
        string strength = EvaluatePasswordStrength(password);

        if (StrengthLabel != null)
        {
            StrengthLabel.Text = strength;

            StrengthLabel.TextColor = strength switch
            {
                "Słabe" => Colors.Red,
                "Średnie" => Colors.Orange,
                "Silne" => Colors.Green,
                _ => Colors.Black
            };
        }
    }
    /********************************************
    nazwa funkcji: EvaluatePasswordStrength
    opis funkcji:  Analizuje hasło pod kątem długości, obecności cyfry oraz wielkiej litery. Zwraca ocenę siły hasła jako tekst.
    parametry:  password – hasło wpisane przez użytkownika
    zwracany typ i opis: string – tekstowa ocena siły hasła: "", "Słabe", "Średnie", "Silne"
    autor: 
    ********************************************/
    private string EvaluatePasswordStrength(string password)
    {
        if (string.IsNullOrEmpty(password))
            return "";

        bool hasDigit = password.Any(char.IsDigit);
        bool hasUpperCase = password.Any(char.IsUpper);

        if (password.Length < 8)
            return "Słabe";

        if (hasDigit && hasUpperCase)
            return "Silne";

        if (hasDigit)
            return "Średnie";

        return "Słabe";
    }
}