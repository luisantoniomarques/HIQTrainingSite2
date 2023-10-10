using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite {
    public class TODO {

        //TODO - // Mudanca do estado da formacao de 0 (por iniciar) para o estado 1 (a decorrer) => todos os dias às 00:01, executar um job que vai buscar todas as datas de inicio das formacoes, se a data de "hoje" for igual à data de inicio ~~> por o estado a 1 (a decorrer), caso contrario, passar ao seguinte
        // ^ database job? need user to do this, cant login with sa_dev...
        //UPDATE dbo.Course SET Status = 1 WHERE StartDate LIKE DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0) - this should work for above

        //Pagina inicial => Ter os objetivos anuais do training (exemplos: numero de horas de formacoes, numero de formacoes realizadas, numero de pessoas que receberam certificacao, numero de formandos, etc)
        // ^ fixed stat for number of hours? + fetch from DB rest

        // Formacoes -> Formandos => relacao entre as formacoes a que se inscreveu e as que cancelou (para que no futuro, no caso de haver poucas vagas, os formandos que nao cancelaram inscrições tenham prioridade sobre as pessoas que cancelam mais)
        // ^ show percentage of signed up/canceled under student name on inscription page?

        // Estatisticas => por ano, pessoa, formacao, area
        //             => % de aulas assistidas/inscritas de um formando
        // ^ Use company dropdown/name input, get all courses if he's a student, believe there is already a call for that?

        // Geral => Passar o tipo de pesquisa nas paginacoes
        // ^ ???

        // Reports => exportar a informacao para excel, pdf  
        // ^ reports of what?

        // Pagina inicial => Ter uma "mapa" com as tarefas individuais de cada elemento do training
        // ???

        //prob more things i didn't see/test

        //also screw TFS for deleting all my code.

    }
}