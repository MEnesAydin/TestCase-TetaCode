import React from 'react';
import { Grade } from '../types';
import { FileText, Trash2, Edit, Calendar, Download, Clock } from 'lucide-react';

interface GradeCardProps {
  grade: Grade;
  onEdit: (grade: Grade) => void;
  onDelete: (id: string) => void;
}

const GradeCard: React.FC<GradeCardProps> = ({ grade, onEdit, onDelete }) => {
  const getFileIcon = () => {
    if (!grade.fileName) return <FileText className="w-6 h-6 text-gray-600" />;
    
    const extension = grade.fileName.split('.').pop()?.toLowerCase();
    
    switch (extension) {
      case 'pdf':
        return <FileText className="w-6 h-6 text-red-600" />;
      case 'doc':
      case 'docx':
        return <FileText className="w-6 h-6 text-blue-600" />;
      case 'xls':
      case 'xlsx':
        return <FileText className="w-6 h-6 text-green-700" />;
      case 'jpg':
      case 'jpeg':
      case 'png':
      case 'gif':
      case 'bmp':
      case 'webp':
        return <FileText className="w-6 h-6 text-green-600" />;
      default:
        return <FileText className="w-6 h-6 text-gray-600" />;
    }
  };

  const getFileTypeName = () => {
    if (!grade.fileName) return 'Dosya';
    
    const extension = grade.fileName.split('.').pop()?.toLowerCase();
    
    switch (extension) {
      case 'pdf':
        return 'PDF';
      case 'doc':
      case 'docx':
        return 'Word';
      case 'xls':
      case 'xlsx':
        return 'Excel';
      case 'jpg':
      case 'jpeg':
      case 'png':
      case 'gif':
      case 'bmp':
      case 'webp':
        return 'Resim';
      default:
        return 'Dosya';
    }
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('tr-TR', {
      day: '2-digit',
      month: 'long',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const handleDownload = async () => {
    if (grade.fileName) {
      try {
        const token = localStorage.getItem('token');
        const fileUrl = `http://localhost:8080/grades/file/${grade.id}`;
        
        const response = await fetch(fileUrl, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        
        if (!response.ok) {
          throw new Error('Download failed');
        }
        
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = grade.fileName;
        document.body.appendChild(link);
        link.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(link);
      } catch (error) {
        console.error('Download error:', error);
        alert('Dosya indirilemedi');
      }
    }
  };

  return (
    <div className="card hover:shadow-md transition-shadow">
      <div className="p-6">
        <div className="flex items-start justify-between mb-4">
          <div className="flex-1">
            <h3 className="text-lg font-semibold text-gray-900 mb-2">{grade.courseName}</h3>
            <p className="text-gray-600 text-sm line-clamp-2">{grade.description}</p>
          </div>
        </div>

        {grade.fileName && (
          <div className="flex items-center space-x-3 mb-4 p-3 bg-gray-50 rounded-lg">
            {getFileIcon()}
            <div className="flex-1 min-w-0">
              <p className="text-sm font-medium text-gray-900 truncate">
                {grade.fileName.split('/').pop()}
              </p>
              <p className="text-xs text-gray-500">{getFileTypeName()}</p>
            </div>
            <button
              onClick={handleDownload}
              className="p-2 text-primary-600 hover:bg-primary-50 rounded-lg transition-colors"
              title="İndir"
            >
              <Download className="w-5 h-5" />
            </button>
          </div>
        )}

        <div className="space-y-1 mb-4">
          <div className="flex items-center text-xs text-gray-500">
            <Calendar className="w-4 h-4 mr-1 flex-shrink-0" />
            <span>{formatDate(grade.createdAt)}</span>
          </div>
          
          {grade.updatedAt && (
            <div className="flex items-center text-xs text-gray-500">
              <Clock className="w-4 h-4 mr-1 flex-shrink-0" />
              <span className="font-medium">Güncellendi:</span>
              <span className="ml-1">{formatDate(grade.updatedAt)}</span>
            </div>
          )}
        </div>

        <div className="flex items-center space-x-2">
          <button
            onClick={() => onEdit(grade)}
            className="flex-1 btn btn-secondary text-sm py-2 flex items-center justify-center space-x-2"
          >
            <Edit className="w-4 h-4" />
            <span>Düzenle</span>
          </button>
          <button
            onClick={() => onDelete(grade.id)}
            className="flex-1 btn btn-danger text-sm py-2 flex items-center justify-center space-x-2"
          >
            <Trash2 className="w-4 h-4" />
            <span>Sil</span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default GradeCard;