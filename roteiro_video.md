# Roteiro do vídeo – AppHotel (BindingContext)

**Duração alvo:** 7 a 8 minutos (limite do trabalho: mín. 5, máx. 10).
**Regras da atividade:** gravação contínua (sem cortes), sua própria voz, sem acelerar, mín. 720p, sem ler texto palavra por palavra.

> Como usar: as frases abaixo são **exemplos de fala**. Leia uma vez, entenda a ideia e depois fale com as suas palavras olhando só para os tópicos em **negrito**. O professor avalia naturalidade (3 pts), então não decore.

**Antes de gravar (prepare a tela):**
- Abra o app rodando (Windows Machine, janela 400x700) ao lado do Visual Studio.
- Deixe abertas as abas: `Contratacao.xaml`, `Contratacao.xaml.cs`, `HospedagemContratada.xaml`, `Hospedagem.cs`, `Quarto.cs`.
- Aumente a fonte do editor (zoom ~150%) para o código ficar legível em 720p.
- OBS Studio em 1280x720 ou 1920x1080, teste o microfone e ative a supressão de ruído.

---

## 1. Abertura (0:00 – 0:40)

**Mostrar:** o app aberto na tela inicial (Cálculo da diária).

> "Olá, pessoal! Meu nome é ___ e neste vídeo vou explicar o funcionamento do **BindingContext** usando o projeto AppHotel que fizemos em sala, na disciplina de Programação para Dispositivos Móveis II da FATEC Jahu.
> Vou mostrar duas coisas principais: primeiro, como o BindingContext permite **vincular elementos da interface entre si**, e depois como ele permite **passar dados de uma tela para outra**.
> No final, falo do que mais gostei e da maior dificuldade que tive."

---

## 2. O problema e a ideia para resolvê-lo (0:40 – 2:00)

**Mostrar:** o app rodando; mexer nos Steppers, escolher suíte e datas.

> "O app é um cálculo de hospedagem. O usuário escolhe a quantidade de adultos e crianças, a suíte e as datas de check-in e check-out. Ao clicar em Avançar, vê uma segunda tela com o resumo e o valor total.
> O problema é: **como levar esses dados da primeira tela para a segunda?** E, antes disso, como mostrar na própria tela o valor do Stepper sem escrever código para atualizar o texto a cada clique?
> A ideia foi criar uma **classe de modelo**, a `Hospedagem`, que guarda todas essas informações e faz o cálculo do valor total. Depois, em vez de ficar copiando valor por valor para os componentes da segunda tela, eu entrego esse objeto inteiro à tela, e os Labels **se ligam sozinhos** às propriedades dele. Essa ligação é o *data binding*, e o que define 'a partir de qual objeto' ela busca os dados é o **BindingContext**."

**Tópico-chave a falar:** BindingContext = "a fonte de dados" daquele elemento (e dos filhos dele). O `{Binding Propriedade}` é o "caminho" dentro dessa fonte.

---

## 3. BindingContext vinculando elementos visuais (2:00 – 3:45)

**Mostrar:** `Contratacao.xaml`, trecho dos Labels de Adultos/Crianças.

```xml
<Label Text="{Binding Value}" BindingContext="{x:Reference stp_adultos}"/>
<Stepper x:Name="stp_adultos" Minimum="0" Maximum="10" Increment="1"/>
```

> "Começando pela primeira tela. Aqui eu quero que o Label ao lado do Stepper mostre o número escolhido.
> Repare no Label: ele tem `Text="{Binding Value}"`. Só que **Value de quem?** É aí que entra o BindingContext: eu defino `BindingContext="{x:Reference stp_adultos}"`, ou seja, a fonte de dados desse Label é o **próprio Stepper**, que eu nomeei com `x:Name`.
> Então o Label procura a propriedade `Value` do Stepper. Quando clico no mais ou no menos, o Stepper muda o `Value` e o Label atualiza **sozinho**, sem nenhuma linha de C#. Vou mostrar funcionando..."

**Mostrar:** clicar no + e no − dos dois Steppers, mostrando o número mudando.

> "Faço o mesmo com as crianças, só trocando a referência para `stp_criancas`.
> **Por que usar isso?** Porque evita código no code-behind só para sincronizar tela. Menos código, menos chance de erro, e a interface sempre reflete o estado real do componente."

**Mostrar rapidamente:** o Picker com `ItemDisplayBinding="{Binding Descricao}"`.

> "Outro uso de binding aqui é no Picker. Ele recebe uma lista de objetos `Quarto` no `ItemsSource`, e o `ItemDisplayBinding` diz qual propriedade de cada quarto deve aparecer na lista: a `Descricao`. Sem isso, o Picker mostraria o nome da classe em vez de 'Suíte Master'."

---

## 4. A classe de modelo (3:45 – 5:00)

**Mostrar:** `Quarto.cs` e depois `Hospedagem.cs`.

> "Para passar dados entre telas, preciso de um objeto que carregue tudo. Esse é o papel da classe `Hospedagem`.
> Ela tem as propriedades: quarto selecionado, quantidade de adultos e crianças, data de check-in e check-out. E tem duas propriedades **calculadas**: a `Estadia`, que é a diferença em dias entre as datas, e o `ValorTotal`, que multiplica adultos e crianças pelo valor da diária de cada um no quarto escolhido e pelo número de dias.
> Também coloquei **validações nos `set`**: se não escolher o quarto, se a quantidade de adultos for zero ou se uma data vier nula, a propriedade lança uma exceção com uma mensagem amigável. Assim a regra de negócio fica no modelo, e não espalhada pela tela.
> E o `Quarto` é uma classe simples: descrição e os valores da diária para adulto e criança."

**Tópico-chave:** o modelo é o que o BindingContext da segunda tela vai receber.

---

## 5. Passando dados de uma tela para outra (5:00 – 6:45)

**Mostrar:** `Contratacao.xaml.cs`, método `Button_Clicked`.

```csharp
Hospedagem h = new() { QntAdultos = ..., QuartoSelecionado = ..., ... };

Navigation.PushAsync(new HospedagemContratada()
{
    BindingContext = h
});
```

> "Agora a parte principal. No clique do botão Avançar, eu crio um objeto `Hospedagem` e preencho com o que o usuário escolheu: pego o `Value` dos Steppers, o item selecionado do Picker e as datas dos DatePickers.
> Depois chamo `Navigation.PushAsync`, que abre a nova tela, e **na hora de criar a página** eu já atribuo `BindingContext = h`. É aqui que o dado 'viaja': a segunda tela nasce já sabendo qual objeto ela deve exibir.
> Tudo isso está dentro de um `try/catch`: se alguma validação do modelo lançar exceção, por exemplo, usuário esquecer de escolher a suíte, eu mostro um `DisplayAlert` com a mensagem, e a navegação **não acontece**."

**Mostrar:** testar sem escolher suíte / com 0 adultos para aparecer o alerta.

**Mostrar:** `HospedagemContratada.xaml`.

```xml
<Label Text="{Binding QuartoSelecionado.Descricao}"/>
<Label Text="{Binding QntAdultos}"/>
<Label Text="{Binding DataCheckin, StringFormat='{}{0:dd/MM/yyyy}'}"/>
<Label Text="{Binding Estadia}"/>
<Label Text="{Binding ValorTotal, StringFormat='{}{0:c}'}"/>
```

> "Na segunda tela, repare que **não tem nenhum BindingContext no XAML e o code-behind está vazio**, só o `InitializeComponent`. Os Labels apenas dizem `{Binding QntAdultos}`, `{Binding ValorTotal}` e assim por diante. Como o BindingContext foi definido na página, **todos os elementos filhos herdam** essa fonte de dados, então cada Binding procura a propriedade dentro do objeto `Hospedagem`.
> Dá para acessar propriedades aninhadas também, como `QuartoSelecionado.Descricao`, que entra no objeto Quarto para pegar o nome da suíte.
> E o `StringFormat` formata o valor: `dd/MM/yyyy` para as datas e `{0:c}` para mostrar o valor total como moeda."

**Mostrar:** rodar o fluxo completo: 2 adultos, 1 criança, Suíte Premium, 3 dias → mostrar o valor.

> "Vou conferir o cálculo: 2 adultos a 150 mais 1 criança a 75, são 375 por dia, vezes 3 dias, dá 1.125. Bateu com a tela."

*(Ajuste os números conforme o teste que você fizer na hora.)*

---

## 6. Justificativa das ferramentas (6:45 – 7:45)

> "Explicando por que escolhi cada ferramenta:
> - **.NET MAUI e XAML**: permitem criar o app com uma única base de código para vários sistemas, e o XAML separa a interface da lógica.
> - **BindingContext e Binding**: são o coração da solução. Eles conectam interface e dados sem eu precisar atualizar cada Label manualmente.
> - **x:Reference e x:Name**: usei para ligar um elemento ao outro, como Label e Stepper.
> - **Classe de modelo (`Hospedagem` e `Quarto`)**: concentra os dados e as regras de cálculo e validação, e é o objeto que viaja entre as telas.
> - **ObservableCollection no Picker**: é a lista de suítes que alimenta o `ItemsSource`; o `ItemDisplayBinding` mostra só a descrição.
> - **Navigation.PushAsync**: abre a segunda tela mantendo a pilha de navegação, assim o usuário consegue voltar.
> - **try/catch com DisplayAlert**: trata os erros de validação do modelo e dá retorno claro ao usuário.
> - **StringFormat**: formata datas e moeda direto na interface."

---

## 7. Encerramento pessoal (7:45 – 8:30)

**Esta parte tem que ser 100% sua.** Fale a verdade; use as perguntas abaixo como guia:

- **O que mais gostou de fazer?** (ex.: ver a tela atualizar sozinha só com o Binding; montar o layout; ver o cálculo funcionando de ponta a ponta)
- **Qual foi a maior dificuldade?** (ex.: entender de quem é o `Value` no Binding; entender que o BindingContext é herdado pelos filhos; lidar com as datas mínimas dos DatePickers; as validações com exceção)
- **Como superou?** (testando, relendo a aula, depurando)

> Exemplo de estrutura: "O que mais gostei foi ___ , porque ___. A maior dificuldade foi ___ , e consegui resolver ___ . Foi isso, pessoal, obrigado por assistir!"

---

## Checklist rápido de pontuação

| Critério | O que garantir |
|---|---|
| Duração (1) | Entre 5 e 10 min |
| Gravação (1,5) | Uma única take, sem cortes, voz sua, sem acelerar |
| Qualidade (1,5) | ≥720p, código legível (zoom), áudio limpo |
| Domínio (3) | Explique com suas palavras, não leia; improvise exemplos |
| Cobertura (3) | BindingContext, vinculação entre elementos (Label↔Stepper), passagem de dados entre telas (`BindingContext = h`), problema/ideia, ferramentas, relato final |

**Entrega:** arquivo .mp4 ou .mkv anexado no Google Classroom até **02/10/26 às 23:59** (sem link de rede social, sem zip/rar).

**Dica:** se errar uma palavra, continue falando e corrija naturalmente. Cortes não são permitidos, e um pequeno tropeço soa mais natural do que um texto lido.
