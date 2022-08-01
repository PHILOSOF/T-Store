using T_Store.Controllers;
using Moq;
using AutoMapper;
using T_Strore.Business.Services;
using T_Store;





public class TransactionControllerTests
{
    private TransactionController _sut;
    private Mock<ITransactionServices> _cleaningObjectsServiceMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _cleaningObjectsServiceMock = new Mock<ITransactionServices>();
        _sut = new TransactionController(_cleaningObjectsServiceMock.Object, _mapper);
    }

}
