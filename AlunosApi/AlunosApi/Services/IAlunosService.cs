using AlunosApi.Models;

namespace AlunosApi.Services
{
    public interface IAlunosService
    {
        Task<List<Aluno>> GetAlunos();
        Task<Aluno> GetAluno(int id);
        Task<List<Aluno>> GetAlunosByNome(string nome);
        Task CreateAluno(Aluno aluno);
        Task UpdateAluno(Aluno aluno);
        Task DeleteAluno(Aluno aluno);
    }
}
