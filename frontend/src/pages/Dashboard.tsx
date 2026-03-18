import React, { useState, useEffect } from 'react';
import { gradeApi } from '../services/api';
import { Grade } from '../types';
import GradeCard from '../components/GradeCard';
import GradeModal from '../components/GradeModal';
import { Plus, Search, ChevronLeft, ChevronRight, Loader } from 'lucide-react';

const Dashboard: React.FC = () => {
  const [grades, setGrades] = useState<Grade[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingGrade, setEditingGrade] = useState<Grade | null>(null);
  const [showDeleted, setShowDeleted] = useState(false);
  const pageSize = 9;

  const fetchGrades = async () => {
    try {
      setLoading(true);
      const response = await gradeApi.getAll(currentPage, pageSize, showDeleted ? true : false);
      console.log('API Response:', response); // Debug için
      
      // Backend ApiResponse<PagedResult<Grade>> döndürüyor
      if (response.isSuccessful && response.data) {
        const pagedData = response.data;
        console.log('Paged Data:', pagedData); // Debug için
        
        // Backend'den gelen yapı: { items: [...], metadata: { pagination: {...} } }
        const items = (pagedData as any).items || [];
        const metadata = (pagedData as any).metadata?.pagination || {};
        
        setGrades(Array.isArray(items) ? items : []);
        setTotalPages(metadata.totalPages || 1);
        setTotalCount(metadata.totalCount || 0);
      } else {
        console.error('API returned unsuccessful response:', response.errorMessages);
        setGrades([]);
      }
    } catch (error) {
      console.error('Failed to fetch grades:', error);
      setGrades([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchGrades();
  }, [currentPage, showDeleted]);

  const handleCreateOrUpdate = async (courseName: string, description: string, file?: File) => {
    try {
      if (editingGrade) {
        const result = await gradeApi.update(editingGrade.id, courseName, description, file);
        if (result.isSuccessful) {
          setIsModalOpen(false);
          setEditingGrade(null);
          setCurrentPage(1);
          await fetchGrades();
        } else {
          alert(result.errorMessages?.[0] || 'Güncelleme başarısız');
        }
      } else {
        const result = await gradeApi.create(courseName, description, file);
        if (result.isSuccessful) {
          setIsModalOpen(false);
          setEditingGrade(null);
          setCurrentPage(1);
          await fetchGrades();
        } else {
          alert(result.errorMessages?.[0] || 'Kaydetme başarısız');
        }
      }
    } catch (error: any) {
      console.error('Failed to save grade:', error);
      const errorMsg = error.response?.data?.errorMessages?.[0] 
        || error.response?.data?.message 
        || error.message 
        || 'İşlem başarısız. Lütfen tekrar deneyin.';
      alert(errorMsg);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await gradeApi.delete(id);
      await fetchGrades();
    } catch (error: any) {
      console.error('Failed to delete grade:', error);
      alert(error.response?.data?.errorMessages?.[0] || 'Silme işlemi başarısız');
    }
  };

  const handleEdit = (grade: Grade) => {
    setEditingGrade(grade);
    setIsModalOpen(true);
  };

  const handleNewGrade = () => {
    setEditingGrade(null);
    setIsModalOpen(true);
  };

  const filteredGrades = Array.isArray(grades) ? grades.filter(
    (grade) =>
      grade.courseName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      grade.description.toLowerCase().includes(searchTerm.toLowerCase())
  ) : [];

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Notlarım</h1>
        <p className="text-gray-600">Ders notlarınızı yönetin ve düzenleyin</p>
      </div>

      <div className="flex flex-col sm:flex-row gap-4 mb-6">
        <div className="flex-1 relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
          <input
            type="text"
            placeholder="Not veya ders ara..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="input pl-10"
          />
        </div>
        
        <div className="flex gap-2">
          <button
            onClick={() => setShowDeleted(!showDeleted)}
            className={`btn ${showDeleted ? 'btn-primary' : 'btn-secondary'}`}
          >
            {showDeleted ? 'Aktif Notlar' : 'Silinmiş Notlar'}
          </button>
          
          <button
            onClick={handleNewGrade}
            className="btn btn-primary flex items-center space-x-2"
          >
            <Plus className="w-5 h-5" />
            <span>Yeni Not</span>
          </button>
        </div>
      </div>

      {loading ? (
        <div className="flex items-center justify-center py-20">
          <Loader className="w-8 h-8 text-primary-600 animate-spin" />
        </div>
      ) : filteredGrades.length === 0 ? (
        <div className="text-center py-20">
          <p className="text-gray-500 text-lg">
            {searchTerm ? 'Arama sonucu bulunamadı' : 'Henüz not eklenmemiş'}
          </p>
          {!searchTerm && (
            <button
              onClick={handleNewGrade}
              className="btn btn-primary mt-4"
            >
              İlk Notunuzu Ekleyin
            </button>
          )}
        </div>
      ) : (
        <>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
            {filteredGrades.map((grade) => (
              <GradeCard
                key={grade.id}
                grade={grade}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
            ))}
          </div>

          {totalPages > 1 && (
            <div className="flex items-center justify-between">
              <p className="text-sm text-gray-600">
                Toplam {totalCount} not - Sayfa {currentPage} / {totalPages}
              </p>
              
              <div className="flex items-center space-x-2">
                <button
                  onClick={() => setCurrentPage(currentPage - 1)}
                  disabled={currentPage === 1}
                  className="btn btn-secondary disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  <ChevronLeft className="w-5 h-5" />
                </button>
                
                <span className="px-4 py-2 text-gray-700 font-medium">
                  {currentPage}
                </span>
                
                <button
                  onClick={() => setCurrentPage(currentPage + 1)}
                  disabled={currentPage === totalPages}
                  className="btn btn-secondary disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  <ChevronRight className="w-5 h-5" />
                </button>
              </div>
            </div>
          )}
        </>
      )}

      <GradeModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingGrade(null);
        }}
        onSubmit={handleCreateOrUpdate}
        grade={editingGrade}
      />
    </div>
  );
};

export default Dashboard;