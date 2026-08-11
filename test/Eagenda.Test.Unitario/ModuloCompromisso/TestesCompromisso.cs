using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;

[TestClass]
public sealed class TestesCompromisso
{
    [TestMethod]
    public void Validar_SemAssunto_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(null, DateTime.Now, new TimeSpan(2), new TimeSpan(5), new TipoCompromisso(), "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Assunto\" deve ser preenchido",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_PoucaLetraAssunto_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso("A", DateTime.Now, new TimeSpan(2), new TimeSpan(5), new TipoCompromisso(), "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Assunto\" deve conter mais que 2 caracteres",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_MuitaLetraAssunto_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 101), DateTime.Now, new TimeSpan(2), new TimeSpan(5), new TipoCompromisso(), "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Assunto\" deve conter 100 ou menos caracteres",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemDataOcorrencia_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), new DateTime(default), new TimeSpan(2), new TimeSpan(5), new TipoCompromisso(), "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Data de Ocorrência\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemHoraInicio_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(default), new TimeSpan(5), new TipoCompromisso(), "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Hora de Início\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemHoraFinal_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(2), new TimeSpan(default), new TipoCompromisso(), "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(2, erros);

        List<string> errosEsperados = [
            "O campo \"Hora de Término\" deve ser preenchido.",
            "A hora de término deve ser posterior à hora de início."
        ];

        CollectionAssert.AreEqual(
            errosEsperados,
            erros
        );

    }

    [TestMethod]
    public void Validar_SemHoraposterior_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(2), new TimeSpan(1), new TipoCompromisso(), "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "A hora de término deve ser posterior à hora de início.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemTipo_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(2), new TimeSpan(5), (TipoCompromisso)67, "casa", "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Tipo de Compromisso\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemLocalPresencial_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(2), new TimeSpan(5), (TipoCompromisso)0, null, "LINKhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Local\" deve ser preenchido para compromissos presenciais.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemLinkRemoto_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(2), new TimeSpan(5), (TipoCompromisso)1, "casa", null, contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Link\" deve ser preenchido para compromissos remotos.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_MaximoLocal_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(2), new TimeSpan(5), (TipoCompromisso)1, new string('A', 267), "Linkhttp", contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Local\" deve conter no máximo 255 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_MaximoLink_DeveRetornar_ErroCorrespondente()
    {
        // Arranjo
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), DateTime.Now, new TimeSpan(2), new TimeSpan(5), (TipoCompromisso)1, "casa", new string('A', 567), contato);

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Link\" deve conter no máximo 500 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Atualizar_Compromisso()
    {
        DateTime dataOcorrencia = DateTime.Now.Date;
        Contato contato = new Contato("viti", "vitu@gmail.com", "49989091739", "DEV", "VitusFirma");
        Compromisso compromisso = new Compromisso(new string('A', 22), dataOcorrencia, TimeSpan.FromHours(2), TimeSpan.FromHours(5), (TipoCompromisso)1, "casa", "LinkHttp", contato);

        Contato contatoAtualizado = new Contato("vitus", "vitus@gmail.com", "49989091738", "Full", "VitusFirma");

        compromisso.Atualizar(new Compromisso("Abacaxi", dataOcorrencia, TimeSpan.FromHours(1), TimeSpan.FromHours(8),
                                    (TipoCompromisso)1, "cabare", "cabareLink", contatoAtualizado));

        Assert.AreEqual("Abacaxi", compromisso.Assunto);
        Assert.AreEqual(dataOcorrencia, compromisso.DataOcorrencia);
        Assert.AreEqual(TimeSpan.FromHours(1), compromisso.HoraInicio);
        Assert.AreEqual(TimeSpan.FromHours(8), compromisso.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Remoto, compromisso.Tipo);
        Assert.AreEqual("cabare", compromisso.Local);
        Assert.AreEqual("cabareLink", compromisso.Link);
        Assert.AreEqual(contatoAtualizado, compromisso.Contato);
    }
}
