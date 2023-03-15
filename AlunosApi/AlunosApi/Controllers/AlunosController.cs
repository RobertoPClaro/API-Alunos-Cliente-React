using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //criar uma autorizçao de uso da controller
    public class AlunosController : ControllerBase
    {
        private readonly IAlunosService _alunosService;

        public AlunosController(IAlunosService alunosService)
        {
            _alunosService = alunosService;
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try
            {
                List<Aluno> alunos = await _alunosService.GetAlunos();

                if (alunos is null)
                {
                    return NotFound();
                }
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao obter alunos");
            }
        }

        [HttpGet("getAlunosByNome")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByNome([FromQuery] string nome)
        {
            try
            {
                List<Aluno> alunos = await _alunosService.GetAlunosByNome(nome);

                if (alunos == null)
                {
                    return NotFound($"Não existem alunos com o criteirio {nome}");
                }
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao obter alunos por nome");
            }
        }

        [HttpGet("{id}", Name = "GetAluno")]
        public async Task<ActionResult<Aluno>> GetAluno(int id)
        {
            try
            {
                Aluno aluno = await _alunosService.GetAluno(id);
                if (aluno == null)
                {
                    return NotFound($"Não existe aluno com o id =  {id}");
                }
                return Ok(aluno);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao obter alunos por id");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAluno(Aluno aluno)
        {
            try
            {
                await _alunosService.CreateAluno(aluno);

                return CreatedAtRoute(nameof(GetAluno), new { id = aluno.Id }, aluno);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                            "Erro ao criar o aluno");

            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAluno(int id, Aluno aluno)
        {
            try
            {
                if (aluno.Id == id)
                {
                    await _alunosService.UpdateAluno(aluno);
                    return Ok($"Aluno com id = {id} atualizado com sucesso");
                }
                else
                {
                    return BadRequest("Dados inconsistentes");
                }
            }
            catch (Exception)
            {

                return BadRequest("Request Invalido");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAluno(int id)
        {
            try
            {
                var aluno = await _alunosService.GetAluno(id);
                if (aluno != null)
                {
                    await _alunosService.DeleteAluno(aluno);
                    return Ok($"Aluno de id = {id} foi excluido com sucesso");
                }
                else
                {
                    return NotFound($"Aluno com id = {id} não foi encontrado");
                }
            }
            catch (Exception)
            {

                return BadRequest("Request Invalido");
            }
        }

    }
}
